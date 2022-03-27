using Earl.Crawler.Middleware.UrlScraping.Abstractions;

namespace Earl.Crawler.Middleware.UrlScraping.Tests;

public sealed class ServiceUrlFilterDescriptorTests
{
    [Fact]
    public void Descriptor_throws_when_type_is_not_url_filter( )
    {
        var exception = Assert.Throws<ArgumentException>( ( ) => new ServiceUrlFilterDescriptor( typeof( ServiceUrlFilterDescriptorTests ) ) );
        Assert.Contains( $"does not implement '{nameof( IUrlFilter )}'.", exception.Message );
    }
}