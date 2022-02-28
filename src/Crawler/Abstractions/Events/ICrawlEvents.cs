namespace Earl.Crawler.Abstractions.Events;

/// <summary> Describes collections of delegates used to broadcast crawl events. </summary>
/// <seealso cref="CrawlEvent"/>
public interface ICrawlEvents
{
    /// <summary> Emit the <typeparamref name="TEvent"/> event. </summary>
    /// <typeparam name="TEvent"> The type of <see cref="CrawlEvent"/> to emit. </typeparam>
    /// <param name="e"> The event. </param>
    /// <param name="cancellation"> A token that cancels the event. </param>
    ValueTask EmitAsync<TEvent>( TEvent e, CancellationToken cancellation = default )
        where TEvent : CrawlEvent;

    /// <summary> Register the given <paramref name="handler"/>. </summary>
    /// <typeparam name="TEvent"> The type of <see cref="CrawlEvent"/> to register a handler for. </typeparam>
    /// <param name="handler"> The <see cref="CrawlEventHandler{TEvent}"/> to register. </param>
    void On<TEvent>( CrawlEventHandler<TEvent> handler )
        where TEvent : CrawlEvent;
}

/// <summary> Describes a method that handles a <see cref="CrawlEvent"/> of type <typeparamref name="TEvent"/>. </summary>
/// <typeparam name="TEvent"> The type of <see cref="CrawlEvent"/> handled. </typeparam>
/// <param name="e"> The <typeparamref name="TEvent"/> to be handled. </param>
/// <param name="cancellation"> A token that cancels the event. </param>
/// <seealso cref="ICrawlEvents"/>
/// <seealso cref="CrawlEvent"/>
public delegate ValueTask CrawlEventHandler<in TEvent>( TEvent e, CancellationToken cancellation )
    where TEvent : CrawlEvent;

/// <summary> Represents an event broadcasted during a crawl. </summary>
/// <seealso cref="CrawlEventHandler{TEvent}"/>
public abstract record CrawlEvent( );

/// <summary> Represents an event broadcasted when an error occurs during a crawl. </summary>
/// <param name="Exception"> The exception that was thrown. </param>
/// <param name="Url"> If the <see cref="Exception"/> was thrown while crawling a url, the said url. </param>
public record CrawlErrorEvent( Exception Exception, Uri? Url = null ) : CrawlEvent;

/// <summary> Represents an event broadcasted when the execution of a crawl progresses. </summary>
/// <param name="CrawledCount"> The number of crawled urls at the time the event was broadcasted. </param>
/// <param name="CurrentQueueLength"> The number of urls enqueued at the time the event was broadcasted. </param>
public record CrawlProgressEvent( int CrawledCount, int CurrentQueueLength ) : CrawlEvent;

/// <summary> Represents an event broadcasted when a crawl has completed for a url. </summary>
/// <param name="Result"> The result of crawling a url. </param>
public record CrawlUrlResultEvent( CrawlUrlResult Result ) : CrawlEvent;

/// <summary> Represents an event broadcasted when the crawler starts crawling a url. </summary>
/// <param name="Url"> The url in which crawling has started. </param>
public record CrawlUrlStartedEvent( Uri Url ) : CrawlEvent;