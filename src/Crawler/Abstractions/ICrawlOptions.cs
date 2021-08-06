using System;

namespace Earl.Crawler.Abstractions
{

    public interface ICrawlOptions
    {

        TimeSpan? BatchDelay { get; }

        int MaxBatchSize { get; }

        /// <summary> The maximum number of concurrent units that may process within the pipeline. </summary>
        int MaxDegreeOfParallelism { get; }

        /// <summary> The maximum number of requests that should be processed. </summary>
        int MaxRequestCount => -1;

        TimeSpan? RequestDelay { get; }

        TimeSpan? Timeout { get; }

    }

}
