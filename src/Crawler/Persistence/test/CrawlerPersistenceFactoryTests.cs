using Earl.Crawler.Abstractions;
using Earl.Crawler.Persistence.Abstractions;
using Earl.Crawler.Persistence.Abstractions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Persistence.Tests;

public sealed class CrawlerPersistenceFactoryTests
{
    [Fact]
    public void Factory_creates_persistence_using_typed_factory( )
    {
        var serviceProvider = new ServiceCollection()
            .AddTransient<CrawlerPersistenceFactory<PersistenceDescriptor>, PersistenceFactory>()
            .BuildServiceProvider();

        var factory = new CrawlerPersistenceFactory( serviceProvider );

        var persistence = factory.Create( new PersistenceDescriptor() );
        Assert.IsType<Persistence>( persistence );
    }

    private sealed class PersistenceDescriptor : ICrawlerPersistenceDescriptor
    {
    }

    private sealed class PersistenceFactory : CrawlerPersistenceFactory<PersistenceDescriptor>
    {
        public override ICrawlerPersistence Create( PersistenceDescriptor descriptor )
            => new Persistence();
    }

    private sealed class Persistence : ICrawlerPersistence
    {
        public Task PersistAsync( CrawlUrlResult result, CancellationToken cancellation = default ) => throw new NotImplementedException();
    }
}