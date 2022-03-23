using Earl.Crawler.Abstractions.Events;

namespace Earl.Crawler.Events;

/// <summary> Default implementation of <see cref="ICrawlerEvents"/>. </summary>
public sealed class CrawlerEvents : ICrawlerEvents
{
    private readonly IDictionary<Type, IList<Delegate>> eventHandlerMap = new Dictionary<Type, IList<Delegate>>();
    private readonly ICrawlerEvents? events;

    public CrawlerEvents( )
    {
    }

    private CrawlerEvents( ICrawlerEvents events )
        => this.events = events;

    /// <inheritdoc/>
    public async ValueTask HandleAsync<TEvent>( TEvent e, CancellationToken cancellation = default )
        where TEvent : CrawlEvent
    {
        ArgumentNullException.ThrowIfNull( e );

        if( events is not null )
        {
            await events.HandleAsync( e, cancellation )
                .ConfigureAwait( false );
        }

        if( eventHandlerMap.TryGetValue( typeof( TEvent ), out var handlers ) )
        {
            await ValueTaskExtensions.WhenAll(
                handlers.Cast<CrawlEventHandler<TEvent>>()
                    .Select( handler => handler( e, cancellation ) )
                    .ToArray()
            );
        }
    }

    /// <summary> Composes an <see cref="ICrawlerEvents"/> instance that contains the given <paramref name="handler"/>. </summary>
    /// <typeparam name="TEvent"> The type of <see cref="CrawlEvent"/> handled. </typeparam>
    /// <param name="events"> The <see cref="ICrawlerEvents"/> to compose. </param>
    /// <param name="handler"> The <see cref="CrawlEventHandler{TEvent}"/> to compose. </param>
    /// <returns> If the given <paramref name="events"/> instance is of type <see cref="CrawlerEvents"/>, the provided instance with the added <paramref name="handler"/>, or a new instance of <see cref="CrawlerEvents"/> with the added <paramref name="handler"/>. </returns>
    public static CrawlerEvents Compose<TEvent>( ICrawlerEvents events, CrawlEventHandler<TEvent> handler )
        where TEvent : CrawlEvent
    {
        ArgumentNullException.ThrowIfNull( events );
        ArgumentNullException.ThrowIfNull( handler );

        var typedEvents = events as CrawlerEvents ?? new CrawlerEvents( events );

        var eventType = typeof( TEvent );
        if( !typedEvents.eventHandlerMap.TryGetValue( eventType, out var handlers ) )
        {
            handlers = typedEvents.eventHandlerMap[ eventType ] = new List<Delegate>();
        }

        handlers.Add( handler );
        return typedEvents;
    }
}