namespace Earl.Crawler.Abstractions;

/// <summary> Represents the result of a crawled url. </summary>
/// <param name="DisplayName"> A friendly display name of the crawl result. </param>
/// <param name="Id"> An unique identifier of the url crawl. </param>
/// <param name="Metadata"> An arbitrary collection of objects representing the data of the crawl result. </param>
/// <param name="Url"> The url that was crawled. </param>
public record CrawlUrlResult
(
    string DisplayName,
    Guid Id,
    IResultMetadataCollection Metadata,
    Uri Url
);