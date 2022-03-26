using Earl.Crawler.Persistence.Configuration;

namespace Earl.Crawler.Persistence.Json.Tests;

public sealed class CrawlerPersistenceBuilderJsonExtensions
{
    [Fact]
    public void Json_persistence_registers_json_persistence_descriptor( )
    {
        var options = new CrawlerPersistenceOptionsBuilder()
            .ToJson( _ => _.Destination( "." ) )
            .Build();

        Assert.Collection(
            options.Descriptors,
            descriptor => Assert.True( descriptor is CrawlerJsonPersistenceDescriptor )
        );
    }
}