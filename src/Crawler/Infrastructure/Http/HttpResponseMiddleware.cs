using System.Net;
using System.Net.Http.Headers;
using Earl.Crawler.Infrastructure.Abstractions;
using Earl.Crawler.Infrastructure.Http.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.IO;

namespace Earl.Crawler.Infrastructure.Http
{

    /// <summary> Supports the <see cref="IHttpResponseFeature"/> by making a HTTP GET request to the current url using <see cref="IEarlHttpClient"/>. </summary>
    public class HttpResponseMiddleware : ICrawlerMiddleware
    {
        #region Fields
        private readonly IEarlHttpClient client;
        private readonly ILogger logger;
        private readonly RecyclableMemoryStreamManager streamManager;
        #endregion

        public HttpResponseMiddleware(
            IEarlHttpClient client,
            ILogger<HttpResponseMiddleware> logger,
            RecyclableMemoryStreamManager streamManager
        )
        {
            this.client = client;
            this.logger = logger;
            this.streamManager = streamManager;
        }

        /// <inheritdoc/>
        public async Task InvokeAsync( CrawlUrlContext context, CrawlUrlDelegate next )
        {
            using var response = await client.GetAsync( context.Url, context.CrawlContext.CrawlAborted );

            var body = streamManager.GetStream( $"{nameof( IHttpResponseFeature )}: {context.Id}" );
            using var source = await response.Content.ReadAsStreamAsync( context.CrawlContext.CrawlAborted )
                .ConfigureAwait( false );

            await source.CopyToAsync( body, context.CrawlContext.CrawlAborted )
                .ConfigureAwait( false );

            var contentHeaders = MapHeaders( response.Content.Headers );
            var headers = MapHeaders( response.Headers );

            var feature = new HttpResponseFeature(
                body,
                contentHeaders,
                headers,
                context.Url,
                new HttpStatistics( response.TotalDuration ),
                response.StatusCode
            );

            context.Features.Set<IHttpResponseFeature?>( feature );

            // context.Result.Metadata.Add( new HttpResponseMetadata() );

            await next( context );
        }

        private static IReadOnlyDictionary<string, StringValues> MapHeaders( HttpHeaders headers )
            => headers.AsEnumerable()
                .ToDictionary(
                    entry => entry.Key,
                    entry => new StringValues( entry.Value.ToArray() )
                );

        private record HttpStatistics( TimeSpan Duration ) : IHttpStatistics;

        private record HttpResponseFeature(
            MemoryStream Body,
            IReadOnlyDictionary<string, StringValues> ContentHeaders,
            IReadOnlyDictionary<string, StringValues> Headers,
            Uri RequestedUrl,
            IHttpStatistics Statistics,
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
