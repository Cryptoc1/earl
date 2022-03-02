namespace Earl.Crawler.Persistence.Json.Configuration;

/// <summary> Describes a builder of <see cref="CrawlerJsonPersistenceOptions"/>. </summary>
public interface ICrawlerJsonPersistenceOptionsBuilder
{
    #region Properties

    /// <summary> A collection of methods used to build the <see cref="CrawlerJsonPersistenceOptions"/>. </summary>
    IList<CrawlerJsonPersistenceOptionsBuildAction> BuildActions { get; }
    #endregion

    /// <summary> Build the <see cref="CrawlerJsonPersistenceOptions"/> for the current state of the builder. </summary>
    CrawlerJsonPersistenceOptions Build( );
}

/// <summary> Describes a method that configures the given <paramref name="options"/> for the given <paramref name="builder"/>. </summary>
/// <param name="builder"> The builder being built. </param>
/// <param name="options"> The options being built. </param>
public delegate CrawlerJsonPersistenceOptions CrawlerJsonPersistenceOptionsBuildAction( ICrawlerJsonPersistenceOptionsBuilder builder, CrawlerJsonPersistenceOptions options );