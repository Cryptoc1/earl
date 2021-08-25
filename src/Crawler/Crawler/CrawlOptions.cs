using Earl.Crawler.Abstractions;

namespace Earl.Crawler
{

    public class CrawlOptions : ICrawlOptions
    {

        /// <inheritdoc/>
        /// <value> 2s (default). </value>
        public virtual TimeSpan? BatchDelay { get; set; } = TimeSpan.FromSeconds( 2 );

        /// <inheritdoc/>
        public virtual int MaxBatchSize { get; set; } = 50;

        /// <inheritdoc/>
        /// <value> <see cref="Environment.ProcessorCount"/><c> /2</c> (default). </value>
        public virtual int MaxDegreeOfParallelism { get; set; } = Environment.ProcessorCount / 2;

        /// <inheritdoc/>
        public virtual int MaxRequestCount { get; set; } = -1;

        /// <inheritdoc/>
        /// <value> .5s (default). </value>
        public virtual TimeSpan? RequestDelay { get; set; } = TimeSpan.FromSeconds( .5 );

        /// <inheritdoc/>
        public virtual TimeSpan? Timeout { get; set; }

    }

}
