using Earl.Crawler.Abstractions;
using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Abstractions.Events;
using Earl.Crawler.Events;
using Earl.Crawler.Middleware.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Middleware.Events.Tests;

public sealed class CrawlUrlContextExtensionsTests
{
    [Fact]
    public async ValueTask Context_handles_error_event( )
    {
        var serviceProvider = new ServiceCollection()
            .BuildServiceProvider();

        bool handled = false;
        var exception = new Exception( "Test" );
        var events = CrawlerEvents.For<CrawlErrorEvent>(
            ( e, _ ) =>
            {
                Assert.Equal( exception, e.Exception );
                handled = true;

                return default;
            }
        );

        var options = new CrawlerOptions( default, default, events, default, default, default!, default );
        var context = new CrawlContext( default!, CancellationToken.None, options, serviceProvider, new(), new() );

        await using var scope = serviceProvider.CreateAsyncScope();
        var urlContext = new CrawlUrlContext( context, default!, default!, scope.ServiceProvider, default! );

        await urlContext.OnErrorAsync( exception );
        Assert.True( handled );
    }

    [Fact]
    public async ValueTask Context_handles_progress_event( )
    {
        var serviceProvider = new ServiceCollection()
            .BuildServiceProvider();

        bool handled = false;
        var events = CrawlerEvents.For<CrawlProgressEvent>(
            ( e, _ ) =>
            {
                handled = true;
                return default;
            }
        );

        var options = new CrawlerOptions( default, default, events, default, default, default!, default );
        var context = new CrawlContext( default!, CancellationToken.None, options, serviceProvider, new(), new() );

        await using var scope = serviceProvider.CreateAsyncScope();
        var urlContext = new CrawlUrlContext( context, default!, default!, scope.ServiceProvider, default! );

        await urlContext.OnProgressAsync();
        Assert.True( handled );
    }

    [Fact]
    public async ValueTask Context_handles_result_event( )
    {
        var serviceProvider = new ServiceCollection()
            .BuildServiceProvider();

        bool handled = false;
        var result = new CrawlUrlResult( "Test", Guid.Empty, default!, default! );

        var events = CrawlerEvents.For<CrawlUrlResultEvent>(
            ( e, _ ) =>
            {
                Assert.Equal( result, e.Result );

                handled = true;
                return default;
            }
        );

        var options = new CrawlerOptions( default, default, events, default, default, default!, default );
        var context = new CrawlContext( default!, CancellationToken.None, options, serviceProvider, new(), new() );

        await using var scope = serviceProvider.CreateAsyncScope();
        var urlContext = new CrawlUrlContext(
            context,
            default!,
            new ResultBuilder( result ),
            scope.ServiceProvider,
            default!
        );

        await urlContext.OnResultAsync();
        Assert.True( handled );
    }

    [Fact]
    public async ValueTask Context_handles_started_event( )
    {
        var serviceProvider = new ServiceCollection()
            .BuildServiceProvider();

        var url = new Uri( "https://localhost:8080/test" );
        bool handled = false;
        var events = CrawlerEvents.For<CrawlUrlStartedEvent>(
            ( e, _ ) =>
            {
                Assert.Equal( url, e.Url );

                handled = true;
                return default;
            }
        );

        var options = new CrawlerOptions( default, default, events, default, default, default!, default );
        var context = new CrawlContext( default!, CancellationToken.None, options, serviceProvider, new(), new() );

        await using var scope = serviceProvider.CreateAsyncScope();
        var urlContext = new CrawlUrlContext( context, default!, default!, scope.ServiceProvider, url );

        await urlContext.OnStartedAsync();
        Assert.True( handled );
    }

    private sealed class ResultBuilder : ICrawlUrlResultBuilder
    {
        public string? DisplayName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Guid Id => throw new NotImplementedException();

        public IList<object> Metadata => throw new NotImplementedException();

        public CrawlUrlResult Result { get; }

        public ResultBuilder( CrawlUrlResult result )
            => Result = result;

        public CrawlUrlResult Build( ) => Result;
    }
}