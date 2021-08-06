using System;
using System.Linq;
using System.Threading.Tasks;
using Earl.Crawler.Infrastructure.Abstractions;
using Earl.Crawler.Infrastructure.Html;
using Earl.Crawler.Infrastructure.UrlScraper.Abstractions;

namespace Earl.Crawler.Infrastructure.UrlScraper
{

    public class UrlScraperMiddleware : ICrawlRequestMiddleware
    {
        #region Fields
        private readonly IUrlScraper scraper;
        #endregion

        public UrlScraperMiddleware( IUrlScraper scraper )
            => this.scraper = scraper;

        public async Task InvokeAsync( CrawlRequestContext context, CrawlRequestDelegate next )
        {
            var documentFeature = context.Features.Get<IHtmlDocumentFeature?>();
            if( documentFeature is null )
            {
                return;
            }

            var urls = await scraper.ScrapeAsync(
                documentFeature.Document,
                new Uri( context.CrawlContext.Initiator.GetLeftPart( UriPartial.Authority ) ),
                context.CrawlContext.CrawlAborted
            );

            if( urls?.Any() is true )
            {
                foreach( var url in urls )
                {
                    if( !context.CrawlContext.Requests.ContainsKey( url ) )
                    {
                        context.CrawlContext.UrlQueue.Enqueue( url );
                    }
                }
            }

            await next( context );
        }

    }

}
