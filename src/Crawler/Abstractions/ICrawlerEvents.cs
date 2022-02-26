namespace Earl.Crawler.Abstractions;

/// <summary> Describes collections of delegates used to broadcast crawl events. </summary>
public interface ICrawlerEvents
{
    /// <summary> A list of <see cref="CrawlErrorEvent"/> handlers. </summary>
    IList<CrawlErrorEvent> OnError { get; }

    /// <summary> A list of <see cref="CrawlResultEvent"/> handlers. </summary>
    IList<CrawlResultEvent> OnResult { get; }

    /// <summary> A list of <see cref="CrawlStartedEvent"/> handlers. </summary>
    IList<CrawlStartedEvent> OnStarted { get; }
}

/// <summary> Describes an event broadcasted when an error occurs while crawling a url. </summary>
/// <param name="url"> The url being crawled when the error occured. </param>
/// <param name="exception"> The <see cref="Exception"/> thrown while crawling. </param>
/// <param name="cancellation"> A token that cancels the event. </param>
public delegate Task CrawlErrorEvent( Uri url, Exception exception, CancellationToken cancellation );

/// <summary> Describes an event broadcasted when the result of crawling a url is available. </summary>
/// <param name="result"> The result of a url crawl. </param>
/// <param name="cancellation"> A token that cancels the event. </param>
public delegate Task CrawlResultEvent( CrawlUrlResult result, CancellationToken cancellation );

/// <summary> Describes an event broadcasted when the crawler starts crawling a url. </summary>
/// <param name="url"> The url being crawled. </param>
/// <param name="cancellation"> A token that cancels the event. </param>
public delegate Task CrawlStartedEvent( Uri url, CancellationToken cancellation );