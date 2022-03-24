using Earl.Crawler.Abstractions.Events;

namespace Earl.Crawler.Events.Tests;

public class CrawlerEventsTests
{
    [Fact]
    public void Compose_does_not_return_empty_events_instance( )
    {
        var events = CrawlerEvents.Compose<TestEvent>( CrawlerEvents.Empty, ( e, _ ) => default );
        Assert.NotEqual( CrawlerEvents.Empty, events );
    }

    [Fact]
    public void Compose_returns_crawler_events_instance( )
    {
        var events = CrawlerEvents.Compose<TestEvent>( new TestCrawlerEvents(), ( e, _ ) => default );
        Assert.IsType<CrawlerEvents>( events );
    }

    [Fact]
    public async Task Composed_events_handle_event_with_original_events( )
    {
        var events = CrawlerEvents.Compose<TestEvent>( new TestCrawlerEvents(), ( e, _ ) => default );
        await Assert.ThrowsAsync<NotImplementedException>(
            async ( ) => await events.HandleAsync<TestEvent>( new( default! ) )
        );
    }

    [Fact]
    public async ValueTask Events_are_handled_for_handler( )
    {
        bool handled = false;
        var events = CrawlerEvents.For<TestEvent>(
            ( e, _ ) =>
            {
                handled = true;
                return ValueTask.CompletedTask;
            }
        );

        await events.HandleAsync( new TestEvent( default! ) );
        Assert.True( handled );
    }

    private sealed class TestCrawlerEvents : ICrawlerEvents
    {
        public ValueTask HandleAsync<TEvent>( TEvent e, CancellationToken cancellation = default )
            where TEvent : CrawlEvent
            => throw new NotImplementedException();
    }

    private sealed record TestEvent( IServiceProvider Services ) : CrawlEvent( Services );
}