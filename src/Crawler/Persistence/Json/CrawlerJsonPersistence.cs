using System.Text.Json;
using Earl.Crawler.Abstractions;
using Earl.Crawler.Persistence.Abstractions;
using Earl.Crawler.Persistence.Json.Configuration;

namespace Earl.Crawler.Persistence.Json;

/// <summary> An implementation of <see cref="ICrawlerPersistence"/> that persists results to JSON files. </summary>
public sealed class CrawlerJsonPersistence : ICrawlerPersistence
{
    private readonly CrawlerJsonPersistenceOptions options;

    public CrawlerJsonPersistence( CrawlerJsonPersistenceOptions options )
        => this.options = options;

    /// <inheritdoc/>
    public async Task PersistAsync( CrawlUrlResult result, CancellationToken cancellation = default )
    {
        string? path = Path.Combine( options.Destination, $"{result.Id}.json" );

        await using var file = new FileStream( path, FileMode.Create, FileAccess.Write );
        await JsonSerializer.SerializeAsync( file, result, options.Serialization, cancellation );
    }
}