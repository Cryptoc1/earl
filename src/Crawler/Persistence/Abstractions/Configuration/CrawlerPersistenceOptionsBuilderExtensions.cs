namespace Earl.Crawler.Persistence.Abstractions.Configuration;

/// <summary> Extensions to <see cref="ICrawlerPersistenceOptionsBuilder"/>. </summary>
public static class CrawlerPersistenceOptionsBuilderExtensions
{
    /// <summary> Adds the given build action to the builder. </summary>
    /// <param name="builder"> The builder to add the <paramref name="configure"/> to. </param>
    /// <param name="configure"> The <see cref="CrawlerPersistenceOptionsBuildAction"/> to add. </param>
    public static ICrawlerPersistenceOptionsBuilder Configure( this ICrawlerPersistenceOptionsBuilder builder, CrawlerPersistenceOptionsBuildAction configure )
    {
        ArgumentNullException.ThrowIfNull( builder );
        ArgumentNullException.ThrowIfNull( configure );

        builder.BuildActions.Add( configure );
        return builder;
    }
}