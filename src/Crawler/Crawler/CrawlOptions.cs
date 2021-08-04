using System;
using Earl.Crawler.Abstractions;

namespace Earl.Crawler
{

    public class CrawlOptions : ICrawlOptions
    {

        public TimeSpan BatchDelay { get; set; } = TimeSpan.FromSeconds( 2 );

        public int MaxBatchSize { get; set; } = 50;

        public int MaxDegreeOfParallelism { get; set; } = Environment.ProcessorCount / 2;

        public int MaxRequestCount { get; set; } = -1;

        public TimeSpan RequestDelay { get; set; } = TimeSpan.FromSeconds( .5 );

        public TimeSpan? Timeout { get; set; }

    }

}
