namespace Earl.Crawler.Infrastructure.Abstractions
{

    /// <summary> Represents the state of a processing url crawl. </summary>
    /// <param name="CrawlContext"> The parent <see cref="Abstractions.CrawlContext"/>. </param>
    /// <param name="Features"> A collection of arbitrary data to expose the functionality of <see cref="ICrawlerMiddleware"/> to down-stream middleware. </param>
    /// <param name="Id"> A unique identifier of the url crawl. </param>
    /// <param name="Services"> A (scoped) <see cref="IServiceProvider"/> that exposes services available to <see cref="ICrawlerMiddleware"/>. </param>
    /// <param name="Url"> The url currently being crawled. </param>
    public record CrawlUrlContext
    (
        CrawlContext CrawlContext,
        ICrawlerFeatureCollection Features,
        Guid Id,
        ICrawlUrlResultBuilder Result,
        IServiceProvider Services,
        Uri Url
    );

}
