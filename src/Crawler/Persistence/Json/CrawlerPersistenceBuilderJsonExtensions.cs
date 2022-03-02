using Earl.Crawler.Persistence.Abstractions.Configuration;
using Earl.Crawler.Persistence.Json.Configuration;

namespace Earl.Crawler.Persistence.Json;

/// <summary> Extensions to <see cref="ICrawlerPersistenceOptionsBuilder"/>. </summary>
public static class CrawlerPersistenceBuilderJsonExtensions
{
    /// <summary> Configures the given <paramref name="builder"/> to persist results to JSON. </summary>
    /// <param name="builder"> The builder to be configured. </param>
    /// <param name="configure"> A delegate that configures an <see cref="ICrawlerJsonPersistenceOptionsBuilder"/>. </param>
    public static ICrawlerPersistenceOptionsBuilder ToJson( this ICrawlerPersistenceOptionsBuilder builder, Action<ICrawlerJsonPersistenceOptionsBuilder> configure )
    {
        ArgumentNullException.ThrowIfNull( builder );
        ArgumentNullException.ThrowIfNull( configure );

        var jsonBuilder = new CrawlerJsonPersistenceOptionsBuilder();
        configure( jsonBuilder );

        var jsonOptions = jsonBuilder.Build();
        return builder.Configure(
            ( _, options ) => options with
            {
                Descriptors = options.Descriptors
                    .Append( new CrawlerJsonPersistenceDescriptor( jsonOptions ) )
                    .ToList()
            }
        );
    }
}