using Earl.Crawler.Abstractions;

namespace Earl.Crawler
{

    public class DelegateCrawlHandler : ICrawlHandler
    {
        #region Fields
        private readonly Func<CrawlUrlResult, CancellationToken, Task> onCrawledUrl;
        #endregion

        public DelegateCrawlHandler( Func<CrawlUrlResult, Task> onCrawlUrlResult )
            : this( ( result, _ ) => onCrawlUrlResult( result ) )
        {
        }

        public DelegateCrawlHandler( Func<CrawlUrlResult, CancellationToken, Task> onCrawledUrl )
            => this.onCrawledUrl = onCrawledUrl;

        public Task OnCrawledUrl( CrawlUrlResult result, CancellationToken cancellation = default )
            => onCrawledUrl( result, cancellation );

    }

}
