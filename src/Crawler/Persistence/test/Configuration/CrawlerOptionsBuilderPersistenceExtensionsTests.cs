using Earl.Crawler.Abstractions;
using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Abstractions.Events;
using Earl.Crawler.Events;
using Earl.Crawler.Persistence.Abstractions;
using Earl.Crawler.Persistence.Abstractions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Persistence.Configuration.Tests;

public sealed class CrawlerOptionsBuilderPersistenceExtensionsTests
{
    [Fact]
    public async Task Persistence_handles_result_event_with_invoker( )
    {
        var options = new OptionsBuilder()
            .PersistTo( persist => { } )
            .Build();

        var invoker = new PersistenceInvoker();
        var serviceProvider = new ServiceCollection()
            .AddTransient<ICrawlerPersistenceInvoker>( _ => invoker )
            .BuildServiceProvider();

        var e = new CrawlUrlResultEvent( default!, serviceProvider );
        await options.Events.HandleAsync( e );

        Assert.True( invoker.Invoked );
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

    private sealed class PersistenceInvoker : ICrawlerPersistenceInvoker
    {
        public bool Invoked { get; set; }

        public Task InvokeAsync( CrawlUrlResult result, CrawlerPersistenceOptions options, CancellationToken cancellation = default )
        {
            Invoked = true;
            return Task.CompletedTask;
        }
    }
}