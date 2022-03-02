namespace Earl.Crawler.Abstractions.Configuration;

/// <summary> Describes a builder of <see cref="CrawlerOptions"/>. </summary>
public interface ICrawlerOptionsBuilder
{
    #region Properties

    /// <summary> A collection of methods used to build the <see cref="CrawlerOptions"/>. </summary>
    IList<CrawlerOptionsBuildAction> BuildActions { get; }
    #endregion

    /// <summary> Build the <see cref="CrawlerOptions"/> for the current state of the builder. </summary>
    CrawlerOptions Build( );
}

/// <summary> Describes a method that configures the given <paramref name="options"/> for the given <paramref name="builder"/>. </summary>
/// <param name="builder"> The builder being built. </param>
/// <param name="options"> The options being built. </param>
public delegate CrawlerOptions CrawlerOptionsBuildAction( ICrawlerOptionsBuilder builder, CrawlerOptions options );