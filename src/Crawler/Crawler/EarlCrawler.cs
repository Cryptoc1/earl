using System.Collections.Concurrent;
using System.Threading.Tasks.Dataflow;
using Axion.Collections.Concurrent;
using Earl.Crawler.Abstractions;
using Earl.Crawler.Middleware.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Earl.Crawler;

/// <summary> Default implementation of <see cref="IEarlCrawler"/>. </summary>
public class EarlCrawler : IEarlCrawler
{
    #region Fields
    private readonly ILogger logger;
    private readonly IServiceProvider serviceProvider;
    #endregion

    public EarlCrawler( ILogger<EarlCrawler> logger, IServiceProvider serviceProvider )
    {
        this.logger = logger;
        this.serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public async Task CrawlAsync( Uri initiator, CrawlerOptions options, CancellationToken cancellation = default )
    {
        ArgumentNullException.ThrowIfNull( initiator );

        using var timeoutSource = options.Timeout.HasValue
            ? new CancellationTokenSource( options.Timeout.Value )
            : null;

        using var cancellationSource = timeoutSource is not null
            ? CancellationTokenSource.CreateLinkedTokenSource( cancellation, timeoutSource.Token )
            : CancellationTokenSource.CreateLinkedTokenSource( cancellation );

        var context = new CrawlContext(
            initiator,
            cancellationSource.Token,
            options,
            new ConcurrentHashSet<Uri>( UriComparer.OrdinalIgnoreCase ),
            new ConcurrentQueue<Uri>( new[] { initiator } )
        );

        logger.LogDebug( "Starting crawl: '{initiator}', {options}.", initiator, options );

        await CrawlAsync( context );

        logger.LogDebug( "Completed crawl" );
    }

    private async Task CrawlAsync( CrawlContext context )
    {
        while( !context.UrlQueue.IsEmpty )
        {
            context.CrawlCancelled.ThrowIfCancellationRequested();
            if( context.HasExceededRequests() )
            {
                break;
            }

            await CrawlBatchedAsync( context );
        }
    }

    private async Task CrawlBatchedAsync( CrawlContext context )
    {
        var processor = new ActionBlock<BatchProcessorContext>(
            async context => await context.Crawler( context.Url, context.Context ),
            new()
            {
                CancellationToken = context.CrawlCancelled,
                EnsureOrdered = false,
                MaxDegreeOfParallelism = Math.Max( 1, context.Options.MaxDegreeOfParallelism )
            }
        );

        var batch = new List<Task>( context.GetEffectiveBatchSize() );
        while( batch.Count < batch.Capacity && context.UrlQueue.TryDequeue( out var url ) )
        {
            context.CrawlCancelled.ThrowIfCancellationRequested();
            if( context.TouchedUrls.Contains( url ) )
            {
                // NOTE: url already crawled, skip
                continue;
            }

            var send = processor.SendAsync( new( CrawlUrlAsync, url, context ), context.CrawlCancelled );
            logger.LogDebug( "Sent '{url}' to crawl processor.", url );

            batch.Add( send );
        }

        await Task.WhenAll( batch ).ConfigureAwait( false );

        processor.Complete();
        await processor.Completion.ConfigureAwait( false );

        logger.LogDebug( "Processed a batch of {count} urls.", batch.Count );
        if( context.Options.BatchDelay.HasValue )
        {
            await Task.Delay( context.Options.BatchDelay.Value, context.CrawlCancelled );
        }
    }

    private async Task CrawlUrlAsync( Uri url, CrawlContext context )
    {
        if( context.TouchedUrls.Contains( url ) )
        {
            return;
        }

        var result = new CrawlUrlResultBuilder( url );

        using var scope = serviceProvider.CreateScope();
        var middleware = scope.ServiceProvider.GetRequiredService<ICrawlerMiddlewareInvoker>();

        using var features = new CrawlerFeatureCollection();
        var urlContext = new CrawlUrlContext( context, features, result, scope.ServiceProvider, url );

        logger.LogDebug( "Invoking middleware for crawl of '{url}', {id}.", result.Id, url );
        await context.Options.Events.OnStartedAsync( url, context.CrawlCancelled );

        try
        {
            await middleware.InvokeAsync( urlContext );
        }
        catch( Exception exception )
        {
            logger.LogError( exception, "Exception encountered during invocation of middleware." );
            await context.Options.Events.OnErrorAsync( url, exception, context.CrawlCancelled );

            return;
        }

        if( context.TouchedUrls.Add( url ) )
        {
            logger.LogDebug( "Touched url '{url}'.", url );
            await context.Options.Events.OnResultAsync( result.Build(), context.CrawlCancelled );
        }
    }

    private record BatchProcessorContext( Func<Uri, CrawlContext, Task> Crawler, Uri Url, CrawlContext Context );
}