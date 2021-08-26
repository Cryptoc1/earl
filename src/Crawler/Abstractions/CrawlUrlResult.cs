namespace Earl.Crawler.Abstractions
{

    /// <summary> Represents the result of a crawled url. </summary>
    /// <param name="Id"> An unique identifier of the url crawl. </param>
    /// <param name="Url"> The url that was crawled. </param>
    public record CrawlUrlResult
    (
        Guid Id,
        IResultMetadataCollection Metadata,
        string Title,
        Uri Url
    );

}
