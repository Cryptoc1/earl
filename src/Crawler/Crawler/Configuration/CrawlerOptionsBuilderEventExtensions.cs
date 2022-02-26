using Earl.Crawler.Abstractions;

namespace Earl.Crawler.Configuration;

/// <summary> Extensions to <see cref="ICrawlerOptionsBuilder"/> for registering <see cref="ICrawlerEvents"/> handlers. </summary>
public static class CrawlerOptionsBuilderEventExtensions
{
    #region Fields
    private static readonly string HandlersKey = nameof( HandlersKey );
    #endregion

    private static void BuildHandlers( ICrawlerOptionsBuilder builder, ICrawlerOptions options )
    {
        ArgumentNullException.ThrowIfNull( builder );
        ArgumentNullException.ThrowIfNull( options );

        var handlers = HandlersProperty( builder );
        mapHandlers( handlers, options.Events.OnError );
        mapHandlers( handlers, options.Events.OnResult );
        mapHandlers( handlers, options.Events.OnStarted );

        static void mapHandlers<TEvent>( IEnumerable<Delegate> source, IList<TEvent> handlers )
            where TEvent : Delegate
        {
            foreach( var handler in source.OfType<TEvent>() )
            {
                handlers.Add( handler );
            }
        }
    }

    private static IList<Delegate> HandlersProperty( ICrawlerOptionsBuilder builder )
       => builder.GetOrAddProperty( HandlersKey, ( ) => new List<Delegate>() );

    /// <summary> Register the given handler. </summary>
    /// <typeparam name="TEvent"> The type of event handler to be registered. </typeparam>
    /// <param name="builder"> The options builder to register the handler with. </param>
    /// <param name="handler"> The event handler to register. </param>
    public static ICrawlerOptionsBuilder WithHandler<TEvent>( this ICrawlerOptionsBuilder builder, TEvent handler )
        where TEvent : Delegate
    {
        ArgumentNullException.ThrowIfNull( builder );
        ArgumentNullException.ThrowIfNull( handler );

        HandlersProperty( builder ).Add( handler );
        return CrawlerOptionsBuilder.Decorate( builder, BuildHandlers );
    }
}