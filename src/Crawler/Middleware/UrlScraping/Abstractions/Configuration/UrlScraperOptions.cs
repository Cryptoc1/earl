namespace Earl.Crawler.Middleware.UrlScraping.Abstractions.Configuration;

/// <summary> Describes the configuration of the <see cref="UrlScraperMiddleware"/>. </summary>
public record UrlScraperOptions( IReadOnlyList<IUrlFilterDescriptor> Filters );