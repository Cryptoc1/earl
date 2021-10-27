using AngleSharp;
using AngleSharp.Html.Dom;
using Earl.Crawler.Middleware.Abstractions;
using Earl.Crawler.Middleware.Http.Abstractions;

namespace Earl.Crawler.Middleware.Html
{

    /// <summary> Supports the <see cref="IHtmlDocumentFeature"/>. </summary>
    public class HtmlDocumentMiddleware : ICrawlerMiddleware
    {

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

            var responseFeature = context.Features.Get<IHttpResponseFeature>();

            var document = await GetDocumentAsync( responseFeature!, context.CrawlContext.CrawlAborted );
            if( document is not null )
            {
                context.Features.Set<IHtmlDocumentFeature?>( new HtmlDocumentFeature( document ) );
            }

            context.Result.DisplayName = $"Crawl Results for '{document!.Title}'";
            await next( context );
        }

        private static async Task<IHtmlDocument?> GetDocumentAsync( IHttpResponseFeature feature, CancellationToken cancellation = default )
        {
            if( feature is null )
            {
                throw new ArgumentNullException( nameof( feature ) );
            }

            var content = feature.Body;
            var document = await BrowsingContext.New()
                .OpenAsync(
                    response =>
                    {
                        response.Status( feature.StatusCode )
                            .Address( feature.RequestedUrl );

                        foreach( var header in feature.Headers )
                        {
                            foreach( var value in header.Value )
                            {
                                response.Header( header.Key, value );
                            }
                        }

                        content.Seek( 0, SeekOrigin.Begin );
                        response.Content( content );
                    },
                    cancellation
                ).ConfigureAwait( false );

            return document as IHtmlDocument;
        }

        private record HtmlDocumentFeature( IHtmlDocument Document ) : IHtmlDocumentFeature, IDisposable
        {

            public void Dispose( )
                => Document?.Dispose();

        }
    }

}
