using Earl.Crawler.Abstractions.Events;

namespace Earl.Crawler.Events;

/// <summary> Extensions to <see cref="ICrawlEvents"/>. </summary>
public static class ICrawlEventsExtensions
{
    private static async Task InvokeAsync<TEvent>( IList<CrawlEventHandler<TEvent>> handlers, TEvent e, CancellationToken cancellation )
        where TEvent : CrawlEvent
    {
        ArgumentNullException.ThrowIfNull( handlers );
        ArgumentNullException.ThrowIfNull( e );

        await Task.WhenAll( handlers.Select( handler => handler( e, cancellation ) ) )
            .ConfigureAwait( false );
    }

    /// <summary> Invokes the <see cref="ICrawlEvents.OnError"/> event handlers. </summary>
    /// <param name="events"> The <see cref="ICrawlEvents"/> instance containing the handlers to invoke. </param>
    /// <param name="e"> The event to broadcast. </param>
    /// <param name="cancellation"> A token that cancels the event. </param>
    /// <seealso cref="CrawlErrorEvent"/>
    public static Task OnErrorAsync( this ICrawlEvents events, CrawlErrorEvent e, CancellationToken cancellation = default )
        => InvokeAsync( events.OnError, e, cancellation );

    /// <summary> Invokes the <see cref="ICrawlEvents.OnProgress"/> event handlers. </summary>
    /// <param name="events"> The <see cref="ICrawlEvents"/> instance containing the handlers to invoke. </param>
    /// <param name="e"> The event to broadcast. </param>
    /// <param name="cancellation"> A token that cancels the event. </param>
    /// <seealso cref="CrawlProgressEvent"/>
    public static Task OnProgressAsync( this ICrawlEvents events, CrawlProgressEvent e, CancellationToken cancellation = default )
        => InvokeAsync( events.OnProgress, e, cancellation );

    /// <summary> Invokes the <see cref="ICrawlEvents.OnUrlResult"/> event handlers. </summary>
    /// <param name="events"> The <see cref="ICrawlEvents"/> instance containing the handlers to invoke. </param>
    /// <param name="e"> The event to broadcast. </param>
    /// <param name="cancellation"> A token that cancels the event. </param>
    /// <seealso cref="CrawlUrlResultEvent"/>
    public static Task OnUrlResultAsync( this ICrawlEvents events, CrawlUrlResultEvent e, CancellationToken cancellation = default )
        => InvokeAsync( events.OnUrlResult, e, cancellation );

    /// <summary> Invokes the <see cref="ICrawlEvents.OnUrlStarted"/> event handlers. </summary>
    /// <param name="events"> The <see cref="ICrawlEvents"/> instance containing the handlers to invoke. </param>
    /// <param name="e"> The event to broadcast. </param>
    /// <param name="cancellation"> A token that cancels the event. </param>
    /// <seealso cref="CrawlUrlStartedEvent"/>
    public static Task OnUrlStartedAsync( this ICrawlEvents events, CrawlUrlStartedEvent e, CancellationToken cancellation = default )
        => InvokeAsync( events.OnUrlStarted, e, cancellation );
}