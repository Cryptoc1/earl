namespace Earl.Crawler.Abstractions
{

    /// <summary> Represents the result of a crawled url. </summary>
    /// <param name="Url"> The url that was crawled. </param>
    /// <param name="Id"> An unique identifier of the url crawl. </param>
    public record CrawlRequestResult
    (
        Uri Url,
        Guid Id
    );

}
