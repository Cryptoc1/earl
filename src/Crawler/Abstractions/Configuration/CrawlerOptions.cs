using Earl.Crawler.Abstractions.Events;

namespace Earl.Crawler.Abstractions.Configuration;

/// <summary> Describes the configuration of a crawl's execution. </summary>
public record CrawlerOptions(
    TimeSpan? BatchDelay,
    int BatchSize,
    ICrawlEvents Events,
    int MaxDegreeOfParallelism,
    int MaxRequestCount,
    IList<ICrawlerMiddlewareDescriptor> Middleware,
    TimeSpan? Timeout
);