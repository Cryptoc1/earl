using System;

namespace Earl.Crawler.Profiles
{

    public class AggressiveCrawlOptions : CrawlOptions
    {

        public override TimeSpan? BatchDelay { get; set; }

        public override int MaxBatchSize { get; set; } = 500;

        public override int MaxDegreeOfParallelism { get; set; } = Environment.ProcessorCount;

        public override TimeSpan? RequestDelay { get; set; } = TimeSpan.FromMilliseconds( 250 );

    }

}
