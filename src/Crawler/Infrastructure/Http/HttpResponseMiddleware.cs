using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Earl.Crawler.Infrastructure.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.IO;

namespace Earl.Crawler.Infrastructure.Http
{

    public class HttpResponseMiddleware : ICrawlRequestMiddleware
    {
        #region Fields
        private readonly IHttpClientFactory clientFactory;
        private readonly ILogger logger;
        private readonly RecyclableMemoryStreamManager streamManager;
        #endregion

        public HttpResponseMiddleware(
            IHttpClientFactory clientFactory,
            ILogger<HttpResponseMiddleware> logger,
            RecyclableMemoryStreamManager streamManager
        )
        {
            this.clientFactory = clientFactory;
            this.logger = logger;
            this.streamManager = streamManager;
        }

        public async Task InvokeAsync( CrawlRequestContext context, CrawlRequestDelegate next )
        {
            using var client = clientFactory.CreateClient();
            using var response = await client.GetAsync( context.Url, context.CrawlContext.CrawlAborted )
                .ConfigureAwait( false );

            var body = streamManager.GetStream( $"{nameof( IHttpResponseFeature )}: {context.Id}" );
            var source = await response.Content.ReadAsStreamAsync( context.CrawlContext.CrawlAborted )
                .ConfigureAwait( false );

            await source.CopyToAsync( body, context.CrawlContext.CrawlAborted )
                .ConfigureAwait( false );

            var contentHeaders = MapHeaders( response.Content.Headers );
            var headers = MapHeaders( response.Headers );

            context.Features.Set<IHttpResponseFeature?>( new HttpResponseFeature( body, contentHeaders, headers, context.Url, response.StatusCode ) );
            logger.LogDebug( $"Set {nameof( IHttpResponseFeature )}: {context.Id}." );

            await next( context );
        }

        private static IReadOnlyDictionary<string, StringValues> MapHeaders( HttpHeaders headers )
            => headers.AsEnumerable()
                .ToDictionary(
                    entry => entry.Key,
                    entry => new StringValues( entry.Value.ToArray() )
                );

        private record HttpResponseFeature(
            Stream Body,
            IReadOnlyDictionary<string, StringValues> ContentHeaders,
            IReadOnlyDictionary<string, StringValues> Headers,
            Uri RequestedUrl,
            HttpStatusCode StatusCode
        ) : IHttpResponseFeature, IAsyncDisposable, IDisposable
        {

            public void Dispose( )
                => Body?.Dispose();

            public async ValueTask DisposeAsync( )
            {
                if( Body is not null )
                {
                    await Body.DisposeAsync();
                }
            }

        }
    }

}
