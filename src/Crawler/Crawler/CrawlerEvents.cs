using Earl.Crawler.Abstractions;

namespace Earl.Crawler;

/// <summary> Default implementation of <see cref="ICrawlerEvents"/> </summary>
public class CrawlerEvents : ICrawlerEvents
{
    /// <inheritdoc/>
    public IList<CrawlErrorEvent> OnError { get; } = new List<CrawlErrorEvent>();

    /// <inheritdoc/>
    public IList<CrawlResultEvent> OnResult { get; } = new List<CrawlResultEvent>();

    /// <inheritdoc/>
    public IList<CrawlStartedEvent> OnStarted { get; } = new List<CrawlStartedEvent>();
}

/// <summary> Extensions to <see cref="ICrawlerEvents"/>. </summary>
public static class ICrawlerEventsExtensions
{
    /// <summary> Invokes the <see cref="ICrawlerEvents.OnError"/> event handlers. </summary>
    /// <param name="events"> The events instance to invoke. </param>
    /// <param name="url"> The url to invoke the handler with. </param>
    /// <param name="exception"> The exception to invoke the handler with. </param>
    /// <param name="cancellation"> The cancellation to invoke the handler with. </param>
    /// <seealso cref="CrawlErrorEvent"/>
    public static async Task OnErrorAsync( this ICrawlerEvents events, Uri url, Exception exception, CancellationToken cancellation = default )
    {
        ArgumentNullException.ThrowIfNull( events );
        foreach( var handler in events.OnError )
        {
            await handler( url, exception, cancellation ).ConfigureAwait( false );
        }
    }

    /// <summary> Invokes the <see cref="ICrawlerEvents.OnResult"/> event handlers. </summary>
    /// <param name="events"> The events instance to invoke. </param>
    /// <param name="result"> The result to invoke the handler with. </param>
    /// <param name="cancellation"> The cancellation to invoke the handler with. </param>
    /// <seealso cref="CrawlResultEvent"/>
    public static async Task OnResultAsync( this ICrawlerEvents events, CrawlUrlResult result, CancellationToken cancellation = default )
    {
        ArgumentNullException.ThrowIfNull( events );
        foreach( var handler in events.OnResult )
        {
            await handler( result, cancellation ).ConfigureAwait( false );
        }
    }

    /// <summary> Invokes the <see cref="ICrawlerEvents.OnError"/> event handlers. </summary>
    /// <param name="events"> The events instance to invoke. </param>
    /// <param name="url"> The url to invoke the handler with. </param>
    /// <param name="cancellation"> The cancellation to invoke the handler with. </param>
    /// <seealso cref="CrawlStartedEvent"/>
    public static async Task OnStartedAsync( this ICrawlerEvents events, Uri url, CancellationToken cancellation = default )
    {
        ArgumentNullException.ThrowIfNull( events );
        foreach( var handler in events.OnStarted )
        {
            await handler( url, cancellation ).ConfigureAwait( false );
        }
    }
}