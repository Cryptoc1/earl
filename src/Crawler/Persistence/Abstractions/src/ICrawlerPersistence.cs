using Earl.Crawler.Abstractions;

namespace Earl.Crawler.Persistence.Abstractions;

/// <summary> Describes a service that can persist a <see cref="CrawlUrlResult"/>. </summary>
public interface ICrawlerPersistence
{
    /// <summary> Perist the given <paramref name="result"/> to the backing storage mechanism. </summary>
    /// <param name="result"> The result to persist. </param>
    /// <param name="cancellation"> A token that cancels the operation. </param>
    Task PersistAsync( CrawlUrlResult result, CancellationToken cancellation = default );
}