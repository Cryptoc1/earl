using Earl.Crawler.Abstractions;

namespace Earl.Crawler.Configuration;

/// <summary> Default <see cref="ICrawlerOptions"/>. </summary>
public class DefaultCrawlerOptions : ICrawlerOptions
{
    /// <inheritdoc/>
    /// <value> 2s (default). </value>
    public virtual TimeSpan? BatchDelay { get; set; } = TimeSpan.FromSeconds( 2 );

    /// <inheritdoc/>
    public virtual ICrawlerEvents Events { get; set; } = new CrawlerEvents();

    /// <inheritdoc/>
    public virtual int MaxBatchSize { get; set; } = 25;

    /// <inheritdoc/>
    /// <value> 2/3s the <see cref="Environment.ProcessorCount"/> (default). </value>
    public virtual int MaxDegreeOfParallelism { get; set; } = Environment.ProcessorCount / 3 * 2;

    /// <inheritdoc/>
    public virtual int MaxRequestCount { get; set; } = -1;

    /// <inheritdoc/>
    public virtual IList<ICrawlerMiddlewareDescriptor> Middleware { get; set; } = new List<ICrawlerMiddlewareDescriptor>();

    /// <inheritdoc/>
    public virtual TimeSpan? Timeout { get; set; }
}