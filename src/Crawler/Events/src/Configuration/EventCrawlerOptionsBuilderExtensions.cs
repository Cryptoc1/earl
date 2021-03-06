using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Abstractions.Events;

namespace Earl.Crawler.Events.Configuration;

/// <summary> Extensions to <see cref="ICrawlerOptionsBuilder"/> for registering <see cref="ICrawlerEvents"/> handlers. </summary>
public static class EventCrawlerOptionsBuilderExtensions
{
    /// <summary> Register the given handler. </summary>
    /// <typeparam name="TEvent"> The type of event handler to be registered. </typeparam>
    /// <param name="builder"> The options builder to register the handler with. </param>
    /// <param name="handler"> The event handler to register. </param>
    public static ICrawlerOptionsBuilder On<TEvent>( this ICrawlerOptionsBuilder builder, CrawlEventHandler<TEvent> handler )
        where TEvent : CrawlEvent
    {
        ArgumentNullException.ThrowIfNull( builder );
        ArgumentNullException.ThrowIfNull( handler );

        return builder.Configure(
            ( _, options ) => options with
            {
                Events = CrawlerEvents.Compose( options.Events, handler ),
            }
        );
    }
}