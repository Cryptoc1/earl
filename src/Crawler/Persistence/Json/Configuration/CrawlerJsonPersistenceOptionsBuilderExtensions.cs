using System.Text.Json;

namespace Earl.Crawler.Persistence.Json.Configuration;

/// <summary> Extensions to <see cref="ICrawlerJsonPersistenceOptionsBuilder"/>. </summary>
public static class CrawlerJsonPersistenceOptionsBuilderExtensions
{
    /// <summary> Configure the folder in which to persist JSON results. </summary>
    /// <param name="builder"> The builder to configure. </param>
    /// <param name="destination"> The destination path. </param>
    public static ICrawlerJsonPersistenceOptionsBuilder Destination( this ICrawlerJsonPersistenceOptionsBuilder builder, string destination )
    {
        ArgumentNullException.ThrowIfNull( builder );
        ArgumentNullException.ThrowIfNull( destination );

        return builder.Configure(
            ( _, options ) => options with { Destination = destination }
        );
    }

    /// <summary> Adds the given build action to the builder. </summary>
    /// <param name="builder"> The builder to add the <paramref name="configure"/> to. </param>
    /// <param name="configure"> The <see cref="CrawlerJsonPersistenceOptionsBuildAction"/> to add. </param>
    public static ICrawlerJsonPersistenceOptionsBuilder Configure( this ICrawlerJsonPersistenceOptionsBuilder builder, CrawlerJsonPersistenceOptionsBuildAction configure )
    {
        ArgumentNullException.ThrowIfNull( builder );
        ArgumentNullException.ThrowIfNull( configure );

        builder.BuildActions.Add( configure );
        return builder;
    }

    /// <summary> Configure the <see cref="JsonSerializerOptions"/> in which to serialize JSON results. </summary>
    /// <param name="builder"> The builder to configure. </param>
    /// <param name="configure"> The delegate method that configures the given <see cref="JsonSerializerOptions"/>. </param>
    public static ICrawlerJsonPersistenceOptionsBuilder Serialize( this ICrawlerJsonPersistenceOptionsBuilder builder, Action<JsonSerializerOptions> configure )
    {
        ArgumentNullException.ThrowIfNull( builder );
        ArgumentNullException.ThrowIfNull( configure );

        return builder.Configure(
            ( _, options ) =>
            {
                var serialization = new JsonSerializerOptions( options.Serialization );
                configure( serialization );

                return options with { Serialization = serialization };
            }
        );
    }
}