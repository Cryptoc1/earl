namespace Earl.Crawler.Abstractions.Events;

/// <summary> Describes a type used to register and emit crawl events. </summary>
/// <seealso cref="CrawlEvent"/>
public interface ICrawlerEvents
{
    /// <summary> Handle the <typeparamref name="TEvent"/> event. </summary>
    /// <typeparam name="TEvent"> The type of <see cref="CrawlEvent"/> to handle. </typeparam>
    /// <param name="e"> The event. </param>
    /// <param name="cancellation"> A token that cancels the event. </param>
    ValueTask HandleAsync<TEvent>( TEvent e, CancellationToken cancellation = default )
        where TEvent : CrawlEvent;
}

/// <summary> Represents an event broadcasted during a crawl. </summary>
/// <param name="Services"> The scoped provider of services to the crawler. Intended for handlers to resolve services required to handle the event. </param>
/// <seealso cref="ICrawlerEvents"/>
public abstract record CrawlEvent( IServiceProvider Services );

/// <summary> Represents an event broadcasted when an error occurs during a crawl. </summary>
/// <param name="Exception"> The exception that was thrown. </param>
/// <param name="Services"> The scoped provider of services to the crawler. Intended for handlers to resolve services required to handle the event. </param>
/// <param name="Url"> If the <see cref="Exception"/> was thrown while crawling a url, the said url. </param>
public record CrawlErrorEvent( Exception Exception, IServiceProvider Services, Uri? Url = null ) : CrawlEvent( Services );

/// <summary> Represents an event broadcasted when the execution of a crawl progresses. </summary>
/// <param name="CrawledCount"> The number of crawled urls at the time the event was broadcasted. </param>
/// <param name="CurrentQueueLength"> The number of urls enqueued at the time the event was broadcasted. </param>
/// <param name="Services"> The scoped provider of services to the crawler. Intended for handlers to resolve services required to handle the event. </param>
/// <param name="Url"> If the event was emitted while crawling a url, the said url. </param>
public record CrawlProgressEvent(
    int CrawledCount,
    int CurrentQueueLength,
    IServiceProvider Services,
    Uri? Url = null
) : CrawlEvent( Services );

/// <summary> Represents an event broadcasted when a crawl has completed for a url. </summary>
/// <param name="Result"> The result of crawling a url. </param>
/// <param name="Services"> The scoped provider of services to the crawler. Intended for handlers to resolve services required to handle the event. </param>
public record CrawlUrlResultEvent( CrawlUrlResult Result, IServiceProvider Services ) : CrawlEvent( Services );

/// <summary> Represents an event broadcasted when the crawler starts crawling a url. </summary>
/// <param name="Services"> The scoped provider of services to the crawler. Intended for handlers to resolve services required to handle the event. </param>
/// <param name="Url"> The url in which crawling has started. </param>
public record CrawlUrlStartedEvent( IServiceProvider Services, Uri Url ) : CrawlEvent( Services );