using Earl.Crawler.Abstractions.Events;

namespace Earl.Crawler.Events;

/// <summary> Default implementation of <see cref="ICrawlEvents"/>. </summary>
public class CrawlEvents : ICrawlEvents
{
    #region Fields
    private readonly IDictionary<Type, IList<Delegate>> eventHandlerMap = new Dictionary<Type, IList<Delegate>>();
    #endregion

    public CrawlEvents( )
    {
    }

    public CrawlEvents( CrawlEvents events )
    {
        foreach( var (key, value) in events.eventHandlerMap )
        {
            eventHandlerMap.Add( key, value );
        }
    }

    /// <inheritdoc/>
    public ValueTask EmitAsync<TEvent>( TEvent e, CancellationToken cancellation = default )
        where TEvent : CrawlEvent
    {
        ArgumentNullException.ThrowIfNull( e );

        if( eventHandlerMap.TryGetValue( typeof( TEvent ), out var handlers ) )
        {
            return ValueTaskExtensions.WhenAll(
                handlers.Cast<CrawlEventHandler<TEvent>>()
                    .Select( handler => handler( e, cancellation ) )
            );
        }

        return ValueTask.CompletedTask;
    }

    /// <inheritdoc/>
    public void On<TEvent>( CrawlEventHandler<TEvent> handler )
        where TEvent : CrawlEvent
    {
        ArgumentNullException.ThrowIfNull( handler );

        var eventType = typeof( TEvent );
        if( !eventHandlerMap.TryGetValue( eventType, out var handlers ) )
        {
            handlers = eventHandlerMap[ eventType ] = new List<Delegate>();
        }

        handlers.Add( handler );
    }
}