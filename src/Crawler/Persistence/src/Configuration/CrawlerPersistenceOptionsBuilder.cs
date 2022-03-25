using Earl.Crawler.Persistence.Abstractions.Configuration;

namespace Earl.Crawler.Persistence.Configuration;

/// <summary> Default implementation of <see cref="ICrawlerPersistenceOptionsBuilder"/>. </summary>
public sealed class CrawlerPersistenceOptionsBuilder : ICrawlerPersistenceOptionsBuilder
{
    /// <inheritdoc/>
    public IList<CrawlerPersistenceOptionsBuildAction> BuildActions { get; } = new List<CrawlerPersistenceOptionsBuildAction>();

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