namespace Earl.Crawler.Abstractions
{

    /// <summary> Describes a crawler, something that crawls urls. </summary>
    public interface IEarlCrawler
    {

        //Task<CrawlResult> CrawlAsync( Uri initiator, ICrawlOptions? options = null, CancellationToken cancellation = default );

        /// <summary> Crawls the specified <paramref name="initiator"/>, using the given <paramref name="options"/>. </summary>
        /// <param name="initiator"> The url to begin the crawl at. </param>
        /// <param name="options"> An object the represents the desired confiuration of the crawl. </param>
        Task CrawlAsync( Uri initiator, ICrawlReporter reporter, ICrawlOptions? options = null, CancellationToken cancellation = default );

    }

    

}
