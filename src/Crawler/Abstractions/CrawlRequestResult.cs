namespace Earl.Crawler.Abstractions
{

    public record CrawlRequestResult
    (
        Uri Url,
        Guid Id
    );

}
