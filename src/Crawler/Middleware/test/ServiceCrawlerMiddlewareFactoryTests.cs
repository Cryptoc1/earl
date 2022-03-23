using Earl.Crawler.Middleware.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Middleware.Tests;

public sealed class ServiceCrawlerMiddlewareFactoryTests
{
    [Fact]
    public void Factory_creates_middleware( )
    {
        var serviceProvider = new ServiceCollection().BuildServiceProvider();

        var factory = new ServiceCrawlerMiddlewareFactory( serviceProvider );

        var descriptor = new ServiceCrawlerMiddlewareDescriptor( typeof( TestMiddleware ) );
        var middleware = factory.Create( descriptor );

        Assert.IsType<TestMiddleware>( middleware );
    }

    [Fact]
    public void Factory_creates_middleware_with_dependency( )
    {
        var serviceProvider = new ServiceCollection()
            .AddTransient<MiddlewareDependency>()
            .BuildServiceProvider();

        var factory = new ServiceCrawlerMiddlewareFactory( serviceProvider );

        var descriptor = new ServiceCrawlerMiddlewareDescriptor( typeof( MiddlewareWithDependency ) );
        var middleware = factory.Create( descriptor );

        var typedMiddleware = Assert.IsType<MiddlewareWithDependency>( middleware );
        Assert.NotNull( typedMiddleware.Dependency );
    }

    [Fact]
    public void Factory_creates_middleware_with_options( )
    {
        var serviceProvider = new ServiceCollection().BuildServiceProvider();
        var factory = new ServiceCrawlerMiddlewareFactory( serviceProvider );

        var descriptor = new ServiceCrawlerMiddlewareDescriptor( typeof( MiddlewareWithOptions ), new MiddlewareOptions( "Test" ) );
        var middleware = factory.Create( descriptor );

        var typedMiddleware = Assert.IsType<MiddlewareWithOptions>( middleware );
        Assert.Equal( "Test", typedMiddleware.Options.Value );
    }

    internal sealed class TestMiddleware : ICrawlerMiddleware
    {
        public Task InvokeAsync( CrawlUrlContext context, CrawlUrlDelegate next ) => throw new NotImplementedException();
    }

    internal sealed class MiddlewareWithDependency : ICrawlerMiddleware
    {
        public MiddlewareDependency Dependency { get; }

        public MiddlewareWithDependency( MiddlewareDependency dependency )
            => Dependency = dependency;

        public Task InvokeAsync( CrawlUrlContext context, CrawlUrlDelegate next ) => throw new NotImplementedException();
    }

    internal sealed class MiddlewareDependency
    {
    }

    internal sealed class MiddlewareWithOptions : ICrawlerMiddleware<MiddlewareOptions>
    {
        public MiddlewareOptions Options { get; }

        public MiddlewareWithOptions( MiddlewareOptions options )
            => Options = options;

        public Task InvokeAsync( CrawlUrlContext context, CrawlUrlDelegate next ) => throw new NotImplementedException();
    }

    internal sealed record MiddlewareOptions( string Value );
}