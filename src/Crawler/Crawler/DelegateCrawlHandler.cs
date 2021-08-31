using Earl.Crawler.Abstractions;

namespace Earl.Crawler
{

    public class DelegateCrawlHandler : ICrawlHandler
    {
        #region Fields
        private readonly Func<CrawlUrlResult, CancellationToken, Task> onCrawlUrlResult;
        #endregion

        public DelegateCrawlHandler( Func<CrawlUrlResult, Task> onCrawlUrlResult )
            : this( ( result, _ ) => onCrawlUrlResult( result ) )
        {
        }

        public DelegateCrawlHandler( Func<CrawlUrlResult, CancellationToken, Task> onCrawlUrlResult )
            => this.onCrawlUrlResult = onCrawlUrlResult;

        public Task OnCrawlUrlResult( CrawlUrlResult result, CancellationToken cancellation = default )
            => onCrawlUrlResult( result, cancellation );

    }

}
