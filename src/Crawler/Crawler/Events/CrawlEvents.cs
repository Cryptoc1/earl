using Earl.Crawler.Abstractions.Events;

namespace Earl.Crawler.Events;

/// <summary> Default implementation of <see cref="ICrawlEvents"/>. </summary>
public class CrawlEvents : ICrawlEvents
{
    /// <inheritdoc/>
    public IList<CrawlEventHandler<CrawlErrorEvent>> OnError { get; } = new List<CrawlEventHandler<CrawlErrorEvent>>();

    /// <inheritdoc/>
    public IList<CrawlEventHandler<CrawlProgressEvent>> OnProgress { get; } = new List<CrawlEventHandler<CrawlProgressEvent>>();

    /// <inheritdoc/>
    public IList<CrawlEventHandler<CrawlUrlResultEvent>> OnUrlResult { get; } = new List<CrawlEventHandler<CrawlUrlResultEvent>>();

    /// <inheritdoc/>
    public IList<CrawlEventHandler<CrawlUrlStartedEvent>> OnUrlStarted { get; } = new List<CrawlEventHandler<CrawlUrlStartedEvent>>();
}