using System.Collections.Concurrent;
using System.Threading.Tasks.Dataflow;
using ConcurrentCollections;
using Earl.Crawler.Abstractions;
using Earl.Crawler.Middleware.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Earl.Crawler
{

    public class EarlCrawler : IEarlCrawler
    {
        #region Fields
        private readonly ILogger logger;
        private readonly IServiceProvider serviceProvider;
        #endregion

        public EarlCrawler(
            ILogger<EarlCrawler> logger,
            IServiceProvider serviceProvider
        )
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        public async Task CrawlAsync( Uri initiator, ICrawlHandler handler, ICrawlOptions? options = null, CancellationToken cancellation = default )
        {
            if( initiator is null )
            {
                throw new ArgumentNullException( nameof( initiator ) );
            }

            if( handler is null )
            {
                throw new ArgumentNullException( nameof( handler ) );
            }

            using var timeoutSource = options?.Timeout is not null
                ? new CancellationTokenSource( options!.Timeout!.Value )
                : null;

            using var abortSource = timeoutSource is not null
                ? CancellationTokenSource.CreateLinkedTokenSource( cancellation, timeoutSource.Token )
                : CancellationTokenSource.CreateLinkedTokenSource( cancellation );

            options ??= new CrawlOptions();
            var context = new CrawlContext(
                initiator,

                abortSource.Token,
                options,
                new ConcurrentHashSet<Uri>( UriComparer.OrdinalIgnoreCase ),
                new ConcurrentQueue<Uri>( new[] { initiator } )
            );

            logger.LogInformation( $"Starting crawl: '{initiator}', {options}." );
            await CrawlAsync( context, handler );
        }

        private async Task CrawlAsync( CrawlContext context, ICrawlHandler handler )
        {
            while( !context.UrlQueue.IsEmpty )
            {
                if( context.Options.MaxRequestCount > 0 )
                {
                    if( context.TouchedUrls.Count == context.Options.MaxRequestCount )
                    {
                        break;
                    }
                }

                context.CrawlAborted.ThrowIfCancellationRequested();

                var batchSize = Math.Max( 1, context.Options.MaxBatchSize );
                var batch = new List<Uri>();

                while( batch.Count < batchSize && context.UrlQueue.TryDequeue( out var url ) )
                {
                    if( context.TouchedUrls.Contains( url ) )
                    {
                        continue;
                    }

                    batch.Add( url );
                }

                if( context.Options.MaxRequestCount > 0 )
                {
                    var remainingRequestCount = Math.Max( 0, context.Options.MaxRequestCount - context.TouchedUrls.Count );

                    // truncate the batch to cap at the `MaxRequestCount`
                    // NOTE: `.Take(int)` safely caps at `batch.Count` when `remainingRequestCount > batch.Count`
                    batch = batch.Take( remainingRequestCount ).ToList();
                }

                if( batch?.Any() is not true )
                {
                    continue;
                }

                var processor = new ActionBlock<(Uri, CrawlContext)>(
                    async request =>
                    {
                        var (url, context) = request;
                        await CrawlUrlAsync( url, context, handler ).ConfigureAwait( false );
                    },
                    new()
                    {
                        CancellationToken = context.CrawlAborted,
                        EnsureOrdered = false,
                        MaxDegreeOfParallelism = Math.Max( 1, context.Options.MaxDegreeOfParallelism )
                    }
                );

                foreach( var url in batch )
                {
                    await processor.SendAsync( (url, context), context.CrawlAborted );
                }

                processor.Complete();
                await processor.Completion;

                if( context.Options.BatchDelay.HasValue )
                {
                    await Task.Delay( context.Options.BatchDelay.Value, context.CrawlAborted );
                }
            }
        }

        private async Task CrawlUrlAsync( Uri url, CrawlContext context, ICrawlHandler handler )
        {
            if( context.TouchedUrls.Contains( url ) )
            {
                // key exists, this url is processed
                return;
            }

            logger.LogDebug( $"Processing Url: '{url}'." );

            var features = new CrawlerFeatureCollection();
            var scope = serviceProvider.CreateScope();

            var id = Guid.NewGuid();
            var result = new CrawlUrlResultBuilder( id, url );

            try
            {
                var middleware = scope.ServiceProvider.GetRequiredService<ICrawlerMiddlewareInvoker>();

                var urlContext = new CrawlUrlContext( context, features, id, result, scope.ServiceProvider, url );
                await middleware.InvokeAsync( urlContext );
            }
            finally
            {
                await features.DisposeAsync();
                features.Dispose();
                scope.Dispose();

                if( context.Options.RequestDelay.HasValue )
                {
                    await Task.Delay( context.Options.RequestDelay.Value, context.CrawlAborted );
                }
            }

            await handler.OnCrawledUrl( result.Build(), context.CrawlAborted );
        }

    }

}
