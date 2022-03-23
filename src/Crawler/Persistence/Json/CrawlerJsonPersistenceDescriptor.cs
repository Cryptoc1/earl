using Earl.Crawler.Persistence.Abstractions;
using Earl.Crawler.Persistence.Abstractions.Configuration;
using Earl.Crawler.Persistence.Json.Configuration;

namespace Earl.Crawler.Persistence.Json;

/// <summary> Represents an <see cref="ICrawlerPersistenceDescriptor"/> for <see cref="CrawlerJsonPersistence"/>. </summary>
public sealed class CrawlerJsonPersistenceDescriptor : ICrawlerPersistenceDescriptor
{
    /// <summary> The configured options for JSON persistence. </summary>
    public CrawlerJsonPersistenceOptions Options { get; }

    public CrawlerJsonPersistenceDescriptor( CrawlerJsonPersistenceOptions options )
        => Options = options;
}

/// <summary> An implementation of <see cref="ICrawlerPersistenceFactory"/> for <see cref="CrawlerJsonPersistenceDescriptor"/>. </summary>
public sealed class CrawlerJsonPersistenceFactory : CrawlerPersistenceFactory<CrawlerJsonPersistenceDescriptor>
{
    /// <inheritdoc/>
    public override ICrawlerPersistence Create( CrawlerJsonPersistenceDescriptor descriptor )
        => new CrawlerJsonPersistence( descriptor.Options );
}