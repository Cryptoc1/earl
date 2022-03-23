using Earl.Crawler.Middleware.Abstractions;

namespace Earl.Crawler.Middleware.Tests;

public sealed class ServiceCrawlerMiddlewareDescriptorTests
{
    [Fact]
    public void Descriptor_throws_when_type_is_not_middleware( )
    {
        var exception = Assert.Throws<ArgumentException>(
            ( ) => new ServiceCrawlerMiddlewareDescriptor( typeof( ServiceCrawlerMiddlewareDescriptorTests ) )
        );

        Assert.Equal( "type", exception.ParamName );
        Assert.Contains( $"does not implement '{nameof( ICrawlerMiddleware )}'", exception.Message );
    }

    [Fact]
    public void Descriptor_throws_when_options_not_given_for_middleware_with_options_type( )
    {
        var exception = Assert.Throws<ArgumentNullException>(
            ( ) => new ServiceCrawlerMiddlewareDescriptor( typeof( MiddlewareWithOptions ) )
        );

        Assert.Equal( "options", exception.ParamName );
    }

    private sealed class MiddlewareWithOptions : ICrawlerMiddleware<MiddlewareOptions>
    {
        public Task InvokeAsync( CrawlUrlContext context, CrawlUrlDelegate next ) => throw new NotImplementedException();
    }

    private sealed record MiddlewareOptions( );
}