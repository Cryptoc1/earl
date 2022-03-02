using System.Text.Json;

namespace Earl.Crawler.Persistence.Json.Configuration;

/// <summary> Default implementation of <see cref="ICrawlerJsonPersistenceOptionsBuilder"/>. </summary>
public class CrawlerJsonPersistenceOptionsBuilder : ICrawlerJsonPersistenceOptionsBuilder
{
    #region Fields
    private string? destination;
    #endregion

    #region Properties

    /// <inheritdoc/>
    public IList<CrawlerJsonPersistenceOptionsBuildAction> BuildActions { get; } = new List<CrawlerJsonPersistenceOptionsBuildAction>();
    #endregion

    /// <inheritdoc/>
    public CrawlerJsonPersistenceOptions Build( )
    {
        if( string.IsNullOrWhiteSpace( destination ) )
        {
            throw new ArgumentNullException( nameof( Destination ) );
        }

        var options = new CrawlerJsonPersistenceOptions( destination, new JsonSerializerOptions( JsonSerializerDefaults.General ) );
        foreach( var buildAction in BuildActions )
        {
            options = buildAction( this, options );
        }

        return options;
    }

    /// <inheritdoc/>
    public ICrawlerJsonPersistenceOptionsBuilder Destination( string destination )
    {
        this.destination = destination;
        return this;
    }
}