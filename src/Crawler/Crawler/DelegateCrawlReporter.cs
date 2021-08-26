using Earl.Crawler.Abstractions;

namespace Earl.Crawler
{

    public class DelegateCrawlReporter : ICrawlReporter
    {
        #region Fields
        private readonly Func<CrawlUrlResult, Task> onCompleteUrlCrawl;
        #endregion

        public DelegateCrawlReporter( Func<CrawlUrlResult, Task> onCompleteUrlCrawl )
            => this.onCompleteUrlCrawl = onCompleteUrlCrawl;

        public Task OnUrlCrawlComplete( CrawlUrlResult result )
            => onCompleteUrlCrawl( result );

    }

}
