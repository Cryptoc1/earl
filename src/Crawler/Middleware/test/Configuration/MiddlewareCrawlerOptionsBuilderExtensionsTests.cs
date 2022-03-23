using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Middleware.Abstractions;

namespace Earl.Crawler.Middleware.Configuration.Tests;

public sealed class MiddlewareCrawlerOptionsBuilderExtensionsTests
{
    [Fact]
    public void Use_registers_delegate_middleware( )
    {
        var middleware = ( CrawlUrlContext context, CrawlUrlDelegate next ) => Task.CompletedTask;
        var options = new OptionsBuilder()
            .Use( middleware )
            .Build();

        var descriptor = options.Middleware.Single();
        var typedDescriptor = Assert.IsType<DelegateCrawlerMiddlewareDescriptor>( descriptor );
        Assert.Equal( middleware, typedDescriptor.Middleware );
    }

    [Fact]
    public void Use_registers_typed_middleware( )
    {
        var options = new OptionsBuilder()
            .Use<Middleware>()
            .Build();

        var descriptor = options.Middleware.Single();
        var typedDescriptor = Assert.IsType<ServiceCrawlerMiddlewareDescriptor>( descriptor );
        Assert.Equal( typeof( Middleware ), typedDescriptor.MiddlewareType );
    }

    [Fact]
    public void Use_registers_typed_middleware_with_options( )
    {
        var middlewareOptions = new MiddlewareOptions();
        var options = new OptionsBuilder()
            .Use<MiddlewareWithOptions, MiddlewareOptions>( middlewareOptions )
            .Build();

        var descriptor = options.Middleware.Single();
        var typedDescriptor = Assert.IsType<ServiceCrawlerMiddlewareDescriptor>( descriptor );
        Assert.Equal( typeof( MiddlewareWithOptions ), typedDescriptor.MiddlewareType );
        Assert.Equal( middlewareOptions, typedDescriptor.Options );
    }

    private sealed class OptionsBuilder : ICrawlerOptionsBuilder
    {
        public IList<CrawlerOptionsBuildAction> BuildActions { get; } = new List<CrawlerOptionsBuildAction>();

        public CrawlerOptions Build( )
        {
            var options = new CrawlerOptions( default, default, default!, default, default, new List<ICrawlerMiddlewareDescriptor>(), default );
            foreach( var action in BuildActions )
            {
                options = action( this, options );
            }

            return options;
        }
    }

    private sealed class Middleware : ICrawlerMiddleware
    {
        public Task InvokeAsync( CrawlUrlContext context, CrawlUrlDelegate next ) => throw new NotImplementedException();
    }

    private sealed class MiddlewareWithOptions : ICrawlerMiddleware<MiddlewareOptions>
    {
        public Task InvokeAsync( CrawlUrlContext context, CrawlUrlDelegate next ) => throw new NotImplementedException();
    }

    private sealed record MiddlewareOptions( );
}