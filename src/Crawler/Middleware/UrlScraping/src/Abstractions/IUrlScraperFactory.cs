using Earl.Crawler.Middleware.UrlScraping.Abstractions.Configuration;

namespace Earl.Crawler.Middleware.UrlScraping.Abstractions;

/// <summary> Describes a service that can create an instance of an <see cref="IUrlScraper"/> for a given <see cref="IUrlScraperDescriptor"/>. </summary>
public interface IUrlScraperFactory
{
    /// <summary> Create an <see cref="IUrlScraper"/> for the given <paramref name="descriptor"/>. </summary>
    /// <param name="descriptor"> The descriptor to be used to create an instance of an <see cref="IUrlScraper"/>. </param>
    IUrlScraper Create( IUrlScraperDescriptor descriptor );
}