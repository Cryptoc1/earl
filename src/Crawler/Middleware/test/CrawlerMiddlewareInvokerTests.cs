using Earl.Crawler.Abstractions;
using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Events;
using Earl.Crawler.Middleware.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Middleware.Tests;

public sealed class CrawlerMiddlewareInvokerTests
{
    [Fact]
    public async Task Invoker_invokes_middleware_in_reverse_order( )
    {
        var serviceProvider = new ServiceCollection()
            .AddTransient<CrawlerMiddlewareFactory<DelegateCrawlerMiddlewareDescriptor>, DelegateCrawlerMiddlewareFactory>()
            .BuildServiceProvider();

        var factory = new CrawlerMiddlewareFactory( serviceProvider );
        var invoker = new CrawlerMiddlewareInvoker( factory );

        int[]? ids = Enumerable.Range( 0, 10 ).ToArray();
        var middleware = ids.Select(
            id => new DelegateCrawlerMiddlewareDescriptor(
                ( context, next ) =>
                {
                    context.Result.Metadata.Add( id );
                    return next( context );
                }
            )
        ).ToList();

        var options = new CrawlerOptions( null, default, CrawlerEvents.Empty, default, default, middleware, null );

        var result = new ResultBuilder();
        var context = new CrawlUrlContext(
            new CrawlContext( default!, CancellationToken.None, options, serviceProvider, new(), new() ),
            default!,
            result,
            serviceProvider,
            default!
        );

        await invoker.InvokeAsync( context );

        Assert.Equal(
            ids.Reverse(),
            result.Metadata.Cast<int>()
        );
    }

    private sealed class ResultBuilder : ICrawlUrlResultBuilder
    {
        public string? DisplayName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Guid Id => throw new NotImplementedException();

        public IList<object> Metadata { get; } = new List<object>();

        public CrawlUrlResult Build( ) => throw new NotImplementedException();
    }
}