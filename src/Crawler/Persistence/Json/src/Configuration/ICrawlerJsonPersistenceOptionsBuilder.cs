namespace Earl.Crawler.Persistence.Json.Configuration;

/// <summary> Describes a builder of <see cref="CrawlerJsonPersistenceOptions"/>. </summary>
public interface ICrawlerJsonPersistenceOptionsBuilder
{
    /// <summary> A collection of methods used to build the <see cref="CrawlerJsonPersistenceOptions"/>. </summary>
    IList<CrawlerJsonPersistenceOptionsBuildAction> BuildActions { get; }

    /// <summary> Build the <see cref="CrawlerJsonPersistenceOptions"/> for the current state of the builder. </summary>
    CrawlerJsonPersistenceOptions Build( );

    /// <summary> Configure the folder in which to persist JSON results. </summary>
    /// <param name="destination"> The destination path. </param>
    ICrawlerJsonPersistenceOptionsBuilder Destination( string destination );
}

/// <summary> Describes a method that configures the given <paramref name="options"/> for the given <paramref name="builder"/>. </summary>
/// <param name="builder"> The builder being built. </param>
/// <param name="options"> The options being built. </param>
public delegate CrawlerJsonPersistenceOptions CrawlerJsonPersistenceOptionsBuildAction( ICrawlerJsonPersistenceOptionsBuilder builder, CrawlerJsonPersistenceOptions options );