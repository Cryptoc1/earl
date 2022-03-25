using System.Text.Json;

namespace Earl.Crawler.Persistence.Json.Tests;

public sealed class CrawlerJsonPersistenceFactoryTests
{
    [Fact]
    public void Factory_creates_json_persistence( )
    {
        var factory = new CrawlerJsonPersistenceFactory();
        var descriptor = new CrawlerJsonPersistenceDescriptor(
            new( string.Empty, new JsonSerializerOptions( JsonSerializerDefaults.General ) )
        );

        var persistence = factory.Create( descriptor );
        Assert.IsType<CrawlerJsonPersistence>( persistence );
    }
}