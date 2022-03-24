using Earl.Crawler.Abstractions.Events;

namespace Earl.Crawler.Events.Tests;

public sealed class ICrawlEventsExtensionsTests
{
    [Fact]
    public async ValueTask Events_handles_error_event( )
    {
        bool handled = false;
        var events = CrawlerEvents.For<CrawlErrorEvent>(
            ( e, _ ) =>
            {
                handled = true;
                return default;
            }
        );

        await events.OnErrorAsync( new( default!, default! ) );
        Assert.True( handled );
    }

    [Fact]
    public async ValueTask Events_handles_progress_event( )
    {
        bool handled = false;
        var events = CrawlerEvents.For<CrawlProgressEvent>(
            ( e, _ ) =>
            {
                handled = true;
                return default;
            }
        );

        await events.OnProgressAsync( new( default, default, default! ) );
        Assert.True( handled );
    }

    [Fact]
    public async ValueTask Events_handles_result_event( )
    {
        bool handled = false;
        var events = CrawlerEvents.For<CrawlUrlResultEvent>(
            ( e, _ ) =>
            {
                handled = true;
                return default;
            }
        );

        await events.OnUrlResultAsync( new( default!, default! ) );
        Assert.True( handled );
    }

    [Fact]
    public async ValueTask Events_handles_started_event( )
    {
        bool handled = false;
        var events = CrawlerEvents.For<CrawlUrlStartedEvent>(
            ( e, _ ) =>
            {
                handled = true;
                return default;
            }
        );

        await events.OnUrlStartedAsync( new( default!, default! ) );
        Assert.True( handled );
    }
}