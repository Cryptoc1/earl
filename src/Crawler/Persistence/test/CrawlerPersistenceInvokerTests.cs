using Earl.Crawler.Abstractions;
using Earl.Crawler.Persistence.Abstractions;
using Earl.Crawler.Persistence.Abstractions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Persistence.Tests;

public sealed class CrawlerPersistenceInvokerTests
{
    [Fact]
    public async Task Invoker_invokes_persistence_in_order( )
    {
        var serviceProvider = new ServiceCollection()
            .AddTransient<CrawlerPersistenceFactory<PersistenceDescriptor>, PersistenceFactory>()
            .BuildServiceProvider();

        var factory = new CrawlerPersistenceFactory( serviceProvider );
        var invoker = new CrawlerPersistenceInvoker( factory );

        int[]? ids = Enumerable.Range( 0, 5 ).ToArray();
        var results = new List<int>();

        var descriptors = ids.Select( id => new PersistenceDescriptor( id, results ) )
            .ToList();

        var result = new CrawlUrlResult( default!, Guid.Empty, default!, default! );
        var options = new CrawlerPersistenceOptions( descriptors );

        await invoker.InvokeAsync( result, options );

        Assert.Equal( ids, results );
    }

    private sealed class PersistenceDescriptor : ICrawlerPersistenceDescriptor
    {
        public int Id { get; }

        public IList<int> Results { get; }

        public PersistenceDescriptor( int id, IList<int> results )
        {
            Id = id;
            Results = results;
        }
    }

    private sealed class PersistenceFactory : CrawlerPersistenceFactory<PersistenceDescriptor>
    {
        public override ICrawlerPersistence Create( PersistenceDescriptor descriptor )
            => new Persistence( descriptor.Id, descriptor.Results );
    }

    private sealed class Persistence : ICrawlerPersistence
    {
        private readonly int id;
        private readonly IList<int> results;

        public Persistence( int id, IList<int> results )
        {
            this.id = id;
            this.results = results;
        }

        public Task PersistAsync( CrawlUrlResult result, CancellationToken cancellation = default )
        {
            results.Add( id );
            return Task.CompletedTask;
        }
    }
}