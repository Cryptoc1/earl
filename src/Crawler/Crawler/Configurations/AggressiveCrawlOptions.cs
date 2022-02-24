namespace Earl.Crawler.Configurations;

public class AggressiveCrawlOptions : CrawlOptions
{
    public override TimeSpan? BatchDelay { get; set; } = null;

    public override int MaxBatchSize { get; set; } = 500;

    public override int MaxDegreeOfParallelism { get; set; } = Environment.ProcessorCount;

    public override TimeSpan? RequestDelay { get; set; } = TimeSpan.FromMilliseconds( 250 );
}