using System.Collections.Concurrent;
using System.Threading.Tasks.Dataflow;
using ConcurrentCollections;
using Earl.Crawler.Abstractions;
using Earl.Crawler.Infrastructure.Abstractions;
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
        public async Task CrawlAsync( Uri initiator, ICrawlReporter reporter, ICrawlOptions? options = null, CancellationToken cancellation = default )
        {
            if( options?.Timeout is not null )
            {
                cancellation = CancellationTokenSource.CreateLinkedTokenSource(
                    cancellation,
                    new CancellationTokenSource( options.Timeout.Value! ).Token
                ).Token;
            }

            options ??= new CrawlOptions();
            var context = new CrawlContext(
                initiator,

                cancellation,
                options,
                new ConcurrentHashSet<Uri>( UriComparer.OrdinalIgnoreCase ),
                new ConcurrentQueue<Uri>( new[] { initiator } )
            );

            logger.LogInformation( $"Starting crawl: '{initiator}', {options}." );
            await CrawlAsync( context, reporter );
        }

        private async Task CrawlAsync( CrawlContext context, ICrawlReporter reporter )
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

                while( batch.Count < batchSize && context.UrlQueue.TryDequeue( out Uri? url ) )
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

                var processor = new ActionBlock<Uri>(
                    async url => await CrawlUrlAsync( url, context, reporter ),
                    new()
                    {
                        CancellationToken = context.CrawlAborted,
                        EnsureOrdered = false,
                        MaxDegreeOfParallelism = Math.Max( 1, context.Options.MaxDegreeOfParallelism )
                    }
                );

                foreach( var url in batch )
                {
                    await processor.SendAsync( url, context.CrawlAborted );
                    if( context.Options.BatchDelay.HasValue )
                    {
                        await Task.Delay( context.Options.BatchDelay.Value, context.CrawlAborted );
                    }
                }

                processor.Complete();
                await processor.Completion;
            }
        }

        private async Task CrawlUrlAsync( Uri url, CrawlContext context, ICrawlReporter reporter )
        {
            if( context.TouchedUrls.Contains( url ) )
            {
                // key exists, this url is processed
                return;
            }

            logger.LogDebug( $"Processing Url: '{url}'." );

            using var features = new CrawlerFeatureCollection();
            using var scope = serviceProvider.CreateScope();

            var id = Guid.NewGuid();
            var urlContext = new CrawlUrlContext(
                context,
                features,
                id,
                new CrawlUrlResultBuilder( id, url ),
                scope.ServiceProvider,
                url
            );

            var middleware = scope.ServiceProvider.GetRequiredService<ICrawlerMiddlewareInvoker>();
            await middleware.InvokeAsync( urlContext );

            if( context.Options.RequestDelay.HasValue )
            {
                await Task.Delay( context.Options.RequestDelay.Value, context.CrawlAborted );
            }

            var result = urlContext.Result.Build();
            await reporter.OnUrlCrawlComplete( result );
        }

    }

}
