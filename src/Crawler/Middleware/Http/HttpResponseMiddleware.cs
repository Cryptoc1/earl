using Earl.Crawler.Middleware.Abstractions;
using Earl.Crawler.Middleware.Http.Abstractions;

namespace Earl.Crawler.Middleware.Http
{

    /// <summary> Supports the <see cref="IHttpResponseFeature"/> by making a HTTP GET request to the current url using <see cref="IEarlHttpClient"/>. </summary>
    public class HttpResponseMiddleware : ICrawlerMiddleware
    {
        #region Fields
        private readonly IEarlHttpClient client;
        #endregion

        public HttpResponseMiddleware( IEarlHttpClient client ) => this.client = client;

        /// <inheritdoc/>
        public async Task InvokeAsync( CrawlUrlContext context, CrawlUrlDelegate next )
        {
            if( context is null )
            {
                throw new ArgumentNullException( nameof( context ) );
            }

            if( next is null )
            {
                throw new ArgumentNullException( nameof( next ) );
            }

            using var response = await client.GetAsync( context.Url, context.CrawlContext.CrawlAborted );

            using var feature = new HttpResponseFeature( response );
            context.Features.Set<IHttpResponseFeature?>( feature );

            await next( context );
        }

        private record HttpResponseFeature( EarlHttpResponseMessage EarlResponse ) : IHttpResponseFeature, IDisposable
        {
            #region Properties
            public HttpResponseMessage Response => EarlResponse;
            #endregion

            public void Dispose( ) => EarlResponse?.Dispose();
        }
    }

}
