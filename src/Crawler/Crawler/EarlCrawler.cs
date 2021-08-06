﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
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
                new ConcurrentDictionary<Uri, CrawlRequestResult?>( UriComparer.OrdinalIgnoreCase ),
                new ConcurrentQueue<Uri>( new[] { initiator } )
            );

            logger.LogInformation( $"Starting crawl: '{initiator}', {options}." );
            await ProcessCrawlAsync( context, cancellation );

            var result = new CrawlResult(
                initiator,
                context.Requests.Select( entry => entry.Value! )
                    .ToList()
                    .AsReadOnly()
            );

            return result;
        }

        private async Task ProcessCrawlAsync( CrawlContext context, CancellationToken cancellation = default )
        {
            while( !context.UrlQueue.IsEmpty )
            {
                if( context.Options.MaxRequestCount > 0 )
                {
                    if( context.Requests.Count == context.Options.MaxRequestCount )
                    {
                        break;
                    }
                }

                cancellation.ThrowIfCancellationRequested();

                var batchSize = Math.Max( 1, context.Options.MaxBatchSize );
                var batch = new List<Uri>();

                while( batch.Count < batchSize && context.UrlQueue.TryDequeue( out Uri? url ) )
                {
                    if( context.Requests.ContainsKey( url ) )
                    {
                        continue;
                    }

                    batch.Add( url );
                }

                if( context.Options.MaxRequestCount > 0 )
                {
                    var remainingRequestCount = Math.Max( 0, context.Options.MaxRequestCount - context.Requests.Count );

                    // truncate the batch to cap at the `MaxRequestCount`
                    // NOTE: `.Take(int)` safely caps at `batch.Count` when `remainingRequestCount > batch.Count`
                    batch = batch.Take( remainingRequestCount ).ToList();
                }

                if( batch?.Any() is not true )
                {
                    continue;
                }

                var processor = new ActionBlock<Uri>(
                    async url => await ProcessUrlAsync( url, context ),
                    new()
                    {
                        CancellationToken = cancellation,
                        EnsureOrdered = false,
                        MaxDegreeOfParallelism = Math.Max( 1, context.Options.MaxDegreeOfParallelism )
                    }
                );

                foreach( var url in batch )
                {
                    await processor.SendAsync( url, cancellation );
                    if( context.Options.BatchDelay.HasValue )
                    {
                        await Task.Delay( context.Options.BatchDelay.Value, cancellation );
                    }
                }

                processor.Complete();
                await processor.Completion;
            }
        }

        private async Task ProcessUrlAsync( Uri url, CrawlContext context )
        {
            if( !context.Requests.TryAdd( url, null ) )
            {
                // key exists, this url is processed
                return;
            }
            
            logger.LogDebug( $"Processing Url: '{url}'." );
            using var scope = serviceProvider.CreateScope();

            using var features = new CrawlRequestFeatureCollection();
            var request = new CrawlRequestContext( context, features, Guid.NewGuid(), scope.ServiceProvider, url );

            var middleware = scope.ServiceProvider.GetRequiredService<ICrawlRequestMiddlewareInvoker>();
            await middleware.InvokeAsync( request );

            var result = new CrawlRequestResult( url );
            if( context.Requests.TryUpdate( url, result, null ) && context.Options.RequestDelay.HasValue )
            {
                await Task.Delay( context.Options.RequestDelay.Value, context.CrawlAborted );
            }
        }

    }

}
