using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Abstractions.Events;
using Earl.Crawler.Events;
using Earl.Crawler.Events.Configuration;
using Earl.Crawler.Persistence.Abstractions;
using Earl.Crawler.Persistence.Abstractions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Persistence.Configuration;

/// <summary> Extensions to <see cref="ICrawlerOptionsBuilder"/> for supporting persistence. </summary>
public static class CrawlerOptionsBuilderPersistenceExtensions
{
    /// <summary> Configures the given <paramref name="builder"/> to persist results. </summary>
    /// <param name="builder"> The <see cref="ICrawlerOptionsBuilder"/> to configure. </param>
    /// <param name="configure"> A delegate that configures an <see cref="ICrawlerPersistenceOptionsBuilder"/>. </param>
    public static ICrawlerOptionsBuilder PersistTo( this ICrawlerOptionsBuilder builder, Action<ICrawlerPersistenceOptionsBuilder> configure )
    {
        ArgumentNullException.ThrowIfNull( builder );
        ArgumentNullException.ThrowIfNull( configure );

        var persistenceBuilder = new CrawlerPersistenceOptionsBuilder();
        configure( persistenceBuilder );

        var handler = CreatePersistenceHandler( persistenceBuilder );
        return builder.On( handler );
    }

    private static CrawlEventHandler<CrawlUrlResultEvent> CreatePersistenceHandler( ICrawlerPersistenceOptionsBuilder builder )
    {
        var options = builder.Build();
        return async ( e, cancellation ) => await e.Services.GetRequiredService<ICrawlerPersistenceInvoker>()
            .InvokeAsync( e.Result, options, cancellation );
    }
}