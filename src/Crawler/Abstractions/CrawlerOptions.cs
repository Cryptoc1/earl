namespace Earl.Crawler.Abstractions;

/// <summary> Describes the configuration of a crawl's execution. </summary>
public record CrawlerOptions(
    TimeSpan? BatchDelay,
    int BatchSize,
    ICrawlerEvents Events,
    int MaxDegreeOfParallelism,
    int MaxRequestCount,
    IList<ICrawlerMiddlewareDescriptor> Middleware,
    TimeSpan? Timeout
);

/// <summary> Describes a type that describes a crawler middleware. </summary>
public interface ICrawlerMiddlewareDescriptor
{
}