namespace Earl.Crawler.Abstractions
{

    /// <summary> Represents the result of crawler all urls, as initiated by <paramref name="Initiator"/>. </summary>
    /// <param name="Initiator"> The initiating url of the crawl. </param>
    /// <param name="Results"> The results of crawling the <paramref name="Initiator"/>. </param>
    public record CrawlResult
    (
        Uri Initiator,
        IResultMetadataCollection Metadata,
        IReadOnlyCollection<CrawlUrlResult> Results
    );

}
