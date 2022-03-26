namespace Earl.Crawler.Middleware.UrlScraping.Abstractions.Configuration;

/// <summary> Describes the configuration of the <see cref="UrlScraperMiddleware"/>. </summary>
/// <param name="Filters"> A collection of objects that describe the configuration of url filters. </param>
/// <param name="Scrapers"> A collection of objects that describe the configuration of url scrapers. </param>
public record UrlScraperOptions(
    IReadOnlyList<IUrlFilterDescriptor> Filters,
    IReadOnlyList<IUrlScraperDescriptor> Scrapers
);