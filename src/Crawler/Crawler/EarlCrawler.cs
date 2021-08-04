using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        public async Task<CrawlResult> CrawlAsync( Uri initiator, ICrawlOptions? options = null, CancellationToken cancellation = default )
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
                new ConcurrentBag<CrawlRequestResult>(),
                new ConcurrentHashSet<Uri>( UriComparer.OrdinalIgnoreCase ),
                new ConcurrentQueue<Uri>( new[] { initiator } )
            );

            logger.LogInformation( $"Starting crawl: '{initiator}', {options}." );
            while( !context.UrlQueue.IsEmpty )
            {
                if( options.MaxRequestCount > 0 )
                {
                    if( context.Results.Count == options.MaxRequestCount )
                    {
                        break;
                    }
                }

                cancellation.ThrowIfCancellationRequested();

                var batchSize = Math.Max( 1, options.MaxBatchSize );
                var batch = new List<Uri>();

                while( batch.Count < batchSize && context.UrlQueue.TryDequeue( out Uri? url ) )
                {
                    if( context.TouchedUrls.Contains( url, UriComparer.OrdinalIgnoreCase ) )
                    {
                        continue;
                    }

                    batch.Add( url );
                }

                if( options.MaxRequestCount > 0 )
                {
                    var remainingRequestCount = Math.Max( 0, options.MaxRequestCount - context.Results.Count );

                    // truncate the batch to cap at the `MaxRequestCount`
                    // NOTE: `.Take(int)` safely caps at `batch.Count` when `remainingRequestCount > batch.Count`
                    batch = batch.Take( remainingRequestCount ).ToList();
                }

                if( batch?.Any() is not true )
                {
                    continue;
                }

                var processor = new ActionBlock<Uri>(
                    async url => await ProcessUrlAsync( url, context, options ),
                    new()
                    {
                        CancellationToken = cancellation,
                        EnsureOrdered = false,
                        MaxDegreeOfParallelism = Math.Max( 1, options.MaxDegreeOfParallelism )
                    }
                );

                foreach( var url in batch )
                {
                    await processor.SendAsync( url, cancellation );
                    await Task.Delay( options.BatchDelay, cancellation );
                }

                processor.Complete();
                await processor.Completion;
            }

            var result = new CrawlResult(
                initiator,
                context.Results.ToList()
                    .AsReadOnly()
            );

            return result;
        }

        private async Task ProcessUrlAsync( Uri url, CrawlContext context, ICrawlOptions options )
        {
            logger.LogDebug( $"Processing Url: '{url}'." );
            context.TouchedUrls.Add( url );

            using var scope = serviceProvider.CreateScope();

            using var features = new CrawlRequestFeatureCollection();
            var request = new CrawlRequestContext( context, features, Guid.NewGuid(), scope.ServiceProvider, url );

            var middleware = scope.ServiceProvider.GetRequiredService<ICrawlRequestMiddlewareInvoker>();
            await middleware.InvokeAsync( request );

            context.Results.Add( new CrawlRequestResult( url ) );

            await Task.Delay( options.RequestDelay, context.CrawlAborted );
        }

    }

}
