using System.Collections.Concurrent;
using System.Threading.Tasks.Dataflow;
using Axion.Collections.Concurrent;
using Earl.Crawler.Abstractions;
using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Events;
using Earl.Crawler.Middleware.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler;

/// <summary> Default implementation of <see cref="IEarlCrawler"/>. </summary>
public class EarlCrawler : IEarlCrawler
{
    #region Fields
    private readonly IServiceProvider serviceProvider;
    #endregion

    public EarlCrawler( IServiceProvider serviceProvider )
        => this.serviceProvider = serviceProvider;

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
            serviceProvider,
            new ConcurrentHashSet<Uri>( UriComparer.OrdinalIgnoreCase ),
            new ConcurrentQueue<Uri>( new[] { initiator } )
        );

        await CrawlAsync( context );
    }

    private static async Task CrawlAsync( CrawlContext context )
    {
        while( !context.UrlQueue.IsEmpty )
        {
            context.CrawlCancelled.ThrowIfCancellationRequested();
            if( context.HasExceededRequests() )
            {
                break;
            }

            await CrawlBatchedAsync( context );
            await context.OnProgressAsync();
        }
    }

    private static async Task CrawlBatchedAsync( CrawlContext context )
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
            batch.Add( send );
        }

        await Task.WhenAll( batch ).ConfigureAwait( false );

        processor.Complete();
        await processor.Completion.ConfigureAwait( false );

        if( context.Options.BatchDelay.HasValue )
        {
            await Task.Delay( context.Options.BatchDelay.Value, context.CrawlCancelled );
        }
    }

    private static async Task CrawlUrlAsync( Uri url, CrawlContext context )
    {
        if( context.TouchedUrls.Contains( url ) )
        {
            return;
        }

        using var features = new CrawlerFeatureCollection();
        var result = new CrawlUrlResultBuilder( url );
        using var scope = context.Services.CreateScope();

        var urlContext = new CrawlUrlContext( context, features, result, scope.ServiceProvider, url );

        await urlContext.OnStartedAsync();
        var middleware = scope.ServiceProvider.GetRequiredService<ICrawlerMiddlewareInvoker>();

        try
        {
            await middleware.InvokeAsync( urlContext );
        }
        catch( Exception exception )
        {
            await urlContext.OnErrorAsync( exception );
            return;
        }

        if( context.TouchedUrls.Add( url ) )
        {
            await urlContext.OnResultAsync( result.Build() );
        }
    }

    private record BatchProcessorContext( Func<Uri, CrawlContext, Task> Crawler, Uri Url, CrawlContext Context );
}