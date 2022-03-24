using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Abstractions.Events;

namespace Earl.Crawler.Events.Configuration.Tests;

public sealed class EventCrawlerOptionsBuilderExtensionsTests
{
    [Fact]
    public void On_registers_handler( )
    {
        var options = new OptionsBuilder()
            .On<TestEvent>( ( _, __ ) => default )
            .Build();

        Assert.NotEqual( CrawlerEvents.Empty, options.Events );
    }

    private sealed class OptionsBuilder : ICrawlerOptionsBuilder
    {
        public IList<CrawlerOptionsBuildAction> BuildActions { get; } = new List<CrawlerOptionsBuildAction>();

        public CrawlerOptions Build( )
        {
            var options = new CrawlerOptions( default, default, CrawlerEvents.Empty, default, default, default!, default );
            foreach( var action in BuildActions )
            {
                options = action( this, options );
            }

            return options;
        }
    }

    private sealed record TestEvent( IServiceProvider _ ) : CrawlEvent( _ );
}