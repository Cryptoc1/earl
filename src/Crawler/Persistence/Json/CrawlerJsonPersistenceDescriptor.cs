using Earl.Crawler.Persistence.Abstractions;
using Earl.Crawler.Persistence.Abstractions.Configuration;
using Earl.Crawler.Persistence.Json.Configuration;

namespace Earl.Crawler.Persistence.Json;

/// <summary> Represents an <see cref="ICrawlerPersistenceDescriptor"/> for <see cref="CrawlerJsonPersistence"/>. </summary>
public class CrawlerJsonPersistenceDescriptor : ICrawlerPersistenceDescriptor
{
    #region Properties
    public CrawlerJsonPersistenceOptions Options { get; }
    #endregion

    public CrawlerJsonPersistenceDescriptor( CrawlerJsonPersistenceOptions options )
        => Options = options;
}

/// <summary> An implementation of <see cref="ICrawlerPersistenceFactory"/> for <see cref="CrawlerJsonPersistenceDescriptor"/>. </summary>
public class CrawlerJsonPersistenceFactory : CrawlerPersistenceFactory<CrawlerJsonPersistenceDescriptor>
{
    /// <inheritdoc/>
    public override ICrawlerPersistence Create( CrawlerJsonPersistenceDescriptor descriptor )
        => new CrawlerJsonPersistence( descriptor.Options );
}