namespace Earl.Crawler.Persistence.Abstractions.Configuration;

/// <summary> Represents the configuration of persistence. </summary>
public record CrawlerPersistenceOptions( IReadOnlyList<ICrawlerPersistenceDescriptor> Providers );