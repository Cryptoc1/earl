namespace Earl.Crawler.Middleware.Tests;

public sealed class DelegateCrawlerMiddlewareDescriptorTests
{
    [Fact]
    public void Descriptor_throws_when_middleware_is_null( )
    {
        var exception = Assert.Throws<ArgumentNullException>( ( ) => new DelegateCrawlerMiddlewareDescriptor( default! ) );
        Assert.Equal( "middleware", exception.ParamName );
    }
}