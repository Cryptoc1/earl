using Earl.Crawler.Abstractions.Events;

namespace Earl.Crawler.Abstractions.Configuration;

/// <summary> Represents the configuration of a crawl's execution. </summary>
public record CrawlerOptions(
    TimeSpan? BatchDelay,
    int BatchSize,
    ICrawlerEvents Events,
    int MaxDegreeOfParallelism,
    int MaxRequestCount,
    IReadOnlyList<ICrawlerMiddlewareDescriptor> Middleware,
    TimeSpan? Timeout
);