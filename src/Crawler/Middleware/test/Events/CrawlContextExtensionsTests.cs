using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Abstractions.Events;
using Earl.Crawler.Events;
using Earl.Crawler.Middleware.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Middleware.Events.Tests;

public class CrawlContextExtensionsTests
{
    [Fact]
    public async ValueTask Context_handles_progress_event( )
    {
        var serviceProvider = new ServiceCollection()
            .BuildServiceProvider();

        bool handled = false;
        var events = CrawlerEvents.For<CrawlProgressEvent>(
            ( e, cancellation ) =>
            {
                handled = true;
                return ValueTask.CompletedTask;
            }
        );

        var options = new CrawlerOptions( default, default, events, default, default, default!, default );
        var context = new CrawlContext( default!, CancellationToken.None, options, serviceProvider, new(), new() );

        await context.OnProgressAsync();
        Assert.True( handled );
    }
}