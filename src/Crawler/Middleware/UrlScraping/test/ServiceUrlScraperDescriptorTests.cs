using Earl.Crawler.Middleware.UrlScraping.Abstractions;

namespace Earl.Crawler.Middleware.UrlScraping.Tests;

public sealed class ServiceUrlScraperDescriptorTests
{
    [Fact]
    public void Descriptor_throws_when_type_is_not_url_scraper( )
    {
        var exception = Assert.Throws<ArgumentException>( ( ) => new ServiceUrlScraperDescriptor( typeof( ServiceUrlScraperDescriptorTests ) ) );
        Assert.Contains( $"does not implement '{nameof( IUrlScraper )}'.", exception.Message );
    }
}