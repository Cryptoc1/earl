using Earl.Crawler.Abstractions.Events;

namespace Earl.Crawler.Events;

/// <summary> Default implementation of <see cref="ICrawlerEvents"/>. </summary>
public sealed class CrawlerEvents : ICrawlerEvents
{
    /// <summary> An instance of <see cref="CrawlerEvents"/> with no handlers. </summary>
    public static readonly CrawlerEvents Empty = new();

    private readonly ICrawlerEvents? events;
    private IDictionary<Type, IList<Delegate>>? eventHandlerMap;

    private CrawlerEvents( )
    {
    }

    private CrawlerEvents( ICrawlerEvents events )
        => this.events = events;

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

        var eventType = typeof( TEvent );
        var typedEvents = events == Empty
            ? new CrawlerEvents()
            : events as CrawlerEvents ?? new CrawlerEvents( events );

        var handlerMap = typedEvents.eventHandlerMap ??= new Dictionary<Type, IList<Delegate>>();
        if( !handlerMap.TryGetValue( eventType, out var handlers ) )
        {
            handlers = handlerMap[ eventType ] = new List<Delegate>();
        }

        handlers.Add( handler );
        return typedEvents;
    }

    /// <summary> Creates an instance for the given <paramref name="handler"/>. </summary>
    /// <typeparam name="TEvent"> The type of event being handled. </typeparam>
    /// <param name="handler"> The <see cref="CrawlEventHandler{TEvent}"/> to initialize an instance for. </param>
    public static CrawlerEvents For<TEvent>( CrawlEventHandler<TEvent> handler )
        where TEvent : CrawlEvent
        => Compose( Empty, handler );

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

        if( eventHandlerMap?.TryGetValue( typeof( TEvent ), out var handlers ) is true )
        {
            var tasks = handlers.Cast<CrawlEventHandler<TEvent>>()
                .Select( async handler => await handler( e, cancellation ).ConfigureAwait( false ) );

            await Task.WhenAll( tasks ).ConfigureAwait( false );
        }
    }
}