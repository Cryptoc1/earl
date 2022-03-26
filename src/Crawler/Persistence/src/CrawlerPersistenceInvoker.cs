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
    public virtual Task InvokeAsync( IReadOnlyList<ICrawlerPersistenceDescriptor> providers, CrawlUrlResult result, CancellationToken cancellation = default )
    {
        ArgumentNullException.ThrowIfNull( providers );
        ArgumentNullException.ThrowIfNull( result );

        return Task.WhenAll(
            providers.Select( persistenceFactory.Create )
                .Select( provider => provider.PersistAsync( result, cancellation ) )
        );
    }
}