using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Abstractions.Events;

namespace Earl.Crawler.Configuration;

/// <summary> Extensions to <see cref="ICrawlerOptionsBuilder"/> for registering <see cref="ICrawlEvents"/> handlers. </summary>
public static class CrawlerOptionsBuilderEventExtensions
{
    private static CrawlerOptions BuildEvents( ICrawlerOptionsBuilder builder, CrawlerOptions options )
    {
        ArgumentNullException.ThrowIfNull( builder );
        ArgumentNullException.ThrowIfNull( options );

        var handlers = EventsProperty( builder );
        mapHandlers( handlers, options.Events.OnError );
        mapHandlers( handlers, options.Events.OnProgress );
        mapHandlers( handlers, options.Events.OnUrlResult );
        mapHandlers( handlers, options.Events.OnUrlStarted );

        static void mapHandlers<TEvent>( IEnumerable<Delegate> source, IList<CrawlEventHandler<TEvent>> handlers )
            where TEvent : CrawlEvent
        {
            foreach( var handler in source.OfType<CrawlEventHandler<TEvent>>() )
            {
                handlers.Add( handler );
            }
        }

        return options;
    }

    private static IList<Delegate> EventsProperty( ICrawlerOptionsBuilder builder )
       => builder.GetOrAddProperty( nameof( CrawlerOptions.Events ), ( ) => new List<Delegate>() );

    /// <summary> Register the given handler. </summary>
    /// <typeparam name="TEvent"> The type of event handler to be registered. </typeparam>
    /// <param name="builder"> The options builder to register the handler with. </param>
    /// <param name="handler"> The event handler to register. </param>
    public static ICrawlerOptionsBuilder On<TEvent>( this ICrawlerOptionsBuilder builder, CrawlEventHandler<TEvent> handler )
        where TEvent : CrawlEvent
    {
        ArgumentNullException.ThrowIfNull( builder );
        ArgumentNullException.ThrowIfNull( handler );

        EventsProperty( builder ).Add( handler );
        return CrawlerOptionsBuilder.Decorate( builder, BuildEvents );
    }
}