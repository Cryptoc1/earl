using Earl.Crawler.Middleware.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Middleware.Tests;

public sealed class CrawlerMiddlewareFactoryTests
{
    [Fact]
    public void Factory_creates_middleware_using_typed_factory( )
    {
        var serviceProvider = new ServiceCollection()
            .AddTransient<CrawlerMiddlewareFactory<ServiceCrawlerMiddlewareDescriptor>, ServiceCrawlerMiddlewareFactory>()
            .BuildServiceProvider();

        var factory = new CrawlerMiddlewareFactory( serviceProvider );

        var middleware = factory.Create( new ServiceCrawlerMiddlewareDescriptor( typeof( TestMiddleware ) ) );
        Assert.IsType<TestMiddleware>( middleware );
    }

    private sealed class TestMiddleware : ICrawlerMiddleware
    {
        public Task InvokeAsync( CrawlUrlContext context, CrawlUrlDelegate next ) => throw new NotImplementedException();
    }
}