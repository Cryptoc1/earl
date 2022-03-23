namespace Earl.Crawler.Persistence.Abstractions.Configuration;

/// <summary> Describes a builder of <see cref="CrawlerPersistenceOptions"/>. </summary>
public interface ICrawlerPersistenceOptionsBuilder
{
    /// <summary> A collection of methods used to build the <see cref="CrawlerPersistenceOptions"/>. </summary>
    IList<CrawlerPersistenceOptionsBuildAction> BuildActions { get; }

    /// <summary> Build the <see cref="CrawlerPersistenceOptions"/> for the current state of the builder. </summary>
    CrawlerPersistenceOptions Build( );
}

/// <summary> Describes a method that configures the given <paramref name="options"/> for the given <paramref name="builder"/>. </summary>
/// <param name="builder"> The builder being built. </param>
/// <param name="options"> The options being built. </param>
public delegate CrawlerPersistenceOptions CrawlerPersistenceOptionsBuildAction( ICrawlerPersistenceOptionsBuilder builder, CrawlerPersistenceOptions options );