using System;
using Earl.Crawler.Abstractions;

namespace Earl.Crawler
{

    public class CrawlOptions : ICrawlOptions
    {

        public virtual TimeSpan? BatchDelay { get; set; } = TimeSpan.FromSeconds( 2 );

        public virtual int MaxBatchSize { get; set; } = 50;

        public virtual int MaxDegreeOfParallelism { get; set; } = Environment.ProcessorCount / 2;

        public virtual int MaxRequestCount { get; set; } = -1;

        public virtual TimeSpan? RequestDelay { get; set; } = TimeSpan.FromSeconds( .5 );

        public virtual TimeSpan? Timeout { get; set; }

    }

}
