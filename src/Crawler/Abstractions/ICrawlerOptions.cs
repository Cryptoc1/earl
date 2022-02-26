namespace Earl.Crawler.Abstractions;

/// <summary> Describes the configuration of a crawl's execution. </summary>
public interface ICrawlerOptions
{
    /// <summary> A delay to be added between processing batches. </summary>
    TimeSpan? BatchDelay { get; }

    /// <summary> An object representing delegate event handlers used to broadcast messages during the execution of a crawl. </summary>
    ICrawlerEvents Events { get; }

    /// <summary> The maximum number of requests to be made per batch. </summary>
    int MaxBatchSize { get; }

    /// <summary> The maximum number of concurrent units that may process within the pipeline. </summary>
    int MaxDegreeOfParallelism { get; }

    /// <summary> The maximum number of requests that should be processed. </summary>
    int MaxRequestCount => -1;

    /// <summary> A collection of <see cref="ICrawlerMiddlewareDescriptor"/>s representing the middleware to be executed while crawling a url.  </summary>
    IList<ICrawlerMiddlewareDescriptor> Middleware { get; }

    /// <summary> A timeout after which the crawl should abort. </summary>
    TimeSpan? Timeout { get; }
}

/// <summary> Describes a type that describes a crawler middleware. </summary>
public interface ICrawlerMiddlewareDescriptor
{
}