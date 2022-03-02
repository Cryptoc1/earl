using Earl.Crawler.Abstractions;
using Earl.Crawler.Persistence.Abstractions;
using Earl.Crawler.Persistence.Abstractions.Configuration;

namespace Earl.Crawler.Persistence;

/// <summary> Default implementation of <see cref="ICrawlerPersistenceInvoker"/>. </summary>
public class CrawlerPersistenceInvoker : ICrawlerPersistenceInvoker
{
    #region Fields
    private readonly ICrawlerPersistenceFactory persistenceFactory;
    #endregion

    public CrawlerPersistenceInvoker( ICrawlerPersistenceFactory persistenceFactory )
        => this.persistenceFactory = persistenceFactory;

    /// <inheritdoc/>
    public virtual async Task InvokeAsync( CrawlUrlResult result, CrawlerPersistenceOptions options, CancellationToken cancellation = default )
    {
        ArgumentNullException.ThrowIfNull( result );
        ArgumentNullException.ThrowIfNull( options );

        var providers = options.Descriptors.Select( persistenceFactory.Create );
        await Task.WhenAll(
            providers.Select( provider => provider.PersistAsync( result, cancellation ) )
        );
    }
}