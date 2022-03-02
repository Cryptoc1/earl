using Earl.Crawler.Persistence.Abstractions.Configuration;

namespace Earl.Crawler.Persistence;

/// <summary> Default implementation of <see cref="ICrawlerPersistenceOptionsBuilder"/>. </summary>
public class CrawlerPersistenceOptionsBuilder : ICrawlerPersistenceOptionsBuilder
{
    #region Properties

    /// <inheritdoc/>
    public IList<CrawlerPersistenceOptionsBuildAction> BuildActions { get; } = new List<CrawlerPersistenceOptionsBuildAction>();
    #endregion

    /// <inheritdoc/>
    public CrawlerPersistenceOptions Build( )
    {
        var options = new CrawlerPersistenceOptions( new List<ICrawlerPersistenceDescriptor>() );
        foreach( var buildAction in BuildActions )
        {
            options = buildAction( this, options );
        }

        return options;
    }
}