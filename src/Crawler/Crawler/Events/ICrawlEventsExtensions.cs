using Earl.Crawler.Abstractions.Events;

namespace Earl.Crawler.Events;

/// <summary> Extensions to <see cref="ICrawlEvents"/>. </summary>
public static class ICrawlEventsExtensions
{
    /// <summary> Invokes the <see cref="CrawlErrorEvent"/> event handlers. </summary>
    /// <param name="events"> The <see cref="ICrawlEvents"/> instance containing the handlers to invoke. </param>
    /// <param name="e"> The event to broadcast. </param>
    /// <param name="cancellation"> A token that cancels the event. </param>
    public static ValueTask OnErrorAsync( this ICrawlEvents events, CrawlErrorEvent e, CancellationToken cancellation = default )
        => events.EmitAsync( e, cancellation );

    /// <summary> Invokes the <see cref="CrawlProgressEvent"/> event handlers. </summary>
    /// <param name="events"> The <see cref="ICrawlEvents"/> instance containing the handlers to invoke. </param>
    /// <param name="e"> The event to broadcast. </param>
    /// <param name="cancellation"> A token that cancels the event. </param>
    public static ValueTask OnProgressAsync( this ICrawlEvents events, CrawlProgressEvent e, CancellationToken cancellation = default )
        => events.EmitAsync( e, cancellation );

    /// <summary> Invokes the <see cref="CrawlUrlResultEvent"/> event handlers. </summary>
    /// <param name="events"> The <see cref="ICrawlEvents"/> instance containing the handlers to invoke. </param>
    /// <param name="e"> The event to broadcast. </param>
    /// <param name="cancellation"> A token that cancels the event. </param>
    public static ValueTask OnUrlResultAsync( this ICrawlEvents events, CrawlUrlResultEvent e, CancellationToken cancellation = default )
        => events.EmitAsync( e, cancellation );

    /// <summary> Invokes the <see cref="CrawlUrlStartedEvent"/> event handlers. </summary>
    /// <param name="events"> The <see cref="ICrawlEvents"/> instance containing the handlers to invoke. </param>
    /// <param name="e"> The event to broadcast. </param>
    /// <param name="cancellation"> A token that cancels the event. </param>
    public static ValueTask OnUrlStartedAsync( this ICrawlEvents events, CrawlUrlStartedEvent e, CancellationToken cancellation = default )
        => events.EmitAsync( e, cancellation );
}