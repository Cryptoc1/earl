using System.Text.Json;

namespace Earl.Crawler.Persistence.Json.Configuration;

/// <summary> Default implementation of <see cref="ICrawlerJsonPersistenceOptionsBuilder"/>. </summary>
public class CrawlerJsonPersistenceOptionsBuilder : ICrawlerJsonPersistenceOptionsBuilder
{
    #region Properties

    /// <inheritdoc/>
    public IList<CrawlerJsonPersistenceOptionsBuildAction> BuildActions { get; } = new List<CrawlerJsonPersistenceOptionsBuildAction>();
    #endregion

    /// <inheritdoc/>
    public CrawlerJsonPersistenceOptions Build( )
    {
        var options = new CrawlerJsonPersistenceOptions( string.Empty, new JsonSerializerOptions( JsonSerializerDefaults.General ) );
        foreach( var buildAction in BuildActions )
        {
            options = buildAction( this, options );
        }

        return options;
    }
}