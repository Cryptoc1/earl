using Earl.Crawler.Abstractions;
using Earl.Crawler.Persistence.Abstractions;
using Earl.Crawler.Persistence.Abstractions.Configuration;

namespace Earl.Crawler.Persistence;

/// <summary> Default implementation of <see cref="ICrawlerPersistenceInvoker"/>. </summary>
public class CrawlerPersistenceInvoker : ICrawlerPersistenceInvoker
{
    private readonly ICrawlerPersistenceFactory persistenceFactory;

    public CrawlerPersistenceInvoker( ICrawlerPersistenceFactory persistenceFactory )
        => this.persistenceFactory = persistenceFactory;

    /// <inheritdoc/>
    public virtual Task InvokeAsync( CrawlUrlResult result, CrawlerPersistenceOptions options, CancellationToken cancellation = default )
    {
        ArgumentNullException.ThrowIfNull( result );
        ArgumentNullException.ThrowIfNull( options );

        return Task.WhenAll(
            options.Descriptors.Select( persistenceFactory.Create )
                .Select( provider => provider.PersistAsync( result, cancellation ) )
        );
    }
}