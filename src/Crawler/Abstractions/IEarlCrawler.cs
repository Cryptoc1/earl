namespace Earl.Crawler.Abstractions
{

    /// <summary> Describes a crawler, something that crawls urls. </summary>
    public interface IEarlCrawler
    {

        /// <summary> Crawls the specified <paramref name="initiator"/>, using the given <paramref name="options"/>. </summary>
        /// <param name="initiator"> The url to begin the crawl at. </param>
        /// <param name="handler"> A service that can handle the result of an url crawl. </param>
        /// <param name="options"> An object the represents the desired confiuration of the crawl. </param>
        Task CrawlAsync( Uri initiator, ICrawlHandler handler, ICrawlOptions? options = null, CancellationToken cancellation = default );

    }



}
