using System.Collections.Concurrent;
using System.Threading.Tasks.Dataflow;
using Axion.Collections.Concurrent;
using Earl.Crawler.Abstractions;
using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Events;
using Earl.Crawler.Middleware.Abstractions;
using Earl.Crawler.Middleware.Events;
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
            context => context.Crawler( context.Url, context.Context ),
            new()
            {
                CancellationToken = context.CrawlCancelled,
                EnsureOrdered = false,
                MaxDegreeOfParallelism = Math.Max( 1, context.Options.MaxDegreeOfParallelism )
            }
        );

        int capacity = context.GetEffectiveBatchSize();
        var batch = new Dictionary<Uri, Task>( capacity, UriComparer.OrdinalIgnoreCase );
        while( batch.Count < capacity && context.UrlQueue.TryDequeue( out var url ) )
        {
            context.CrawlCancelled.ThrowIfCancellationRequested();
            if( batch.ContainsKey( url ) || context.TouchedUrls.Contains( url ) )
            {
                // NOTE: url already crawled, skip
                continue;
            }

            batch[ url ] = processor.SendAsync( new( CrawlUrlAsync, url, context ), context.CrawlCancelled );
        }

        await Task.WhenAll( batch.Values ).ConfigureAwait( false );

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

        var result = new CrawlUrlResultBuilder( url );
        await using var features = new CrawlerFeatureCollection();
        await using var scope = context.Services.CreateAsyncScope();

        var urlContext = new CrawlUrlContext( context, features, result, scope.ServiceProvider, url );

        await urlContext.OnStartedAsync();
        var middleware = scope.ServiceProvider.GetRequiredService<ICrawlerMiddlewareInvoker>();

        try
        {
            await middleware.InvokeAsync( urlContext )
                .ConfigureAwait( false );
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