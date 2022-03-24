using Earl.Crawler.Abstractions.Events;

namespace Earl.Crawler.Events.Tests;

public class CrawlerEventsTests
{
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

    private sealed record TestEvent( IServiceProvider Services ) : CrawlEvent( Services );
}