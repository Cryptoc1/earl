namespace Earl.Crawler.Persistence.Json.Tests;

public sealed class CrawlerJsonPersistenceDescriptorTests
{
    [Fact]
    public void Descriptor_throws_when_options_are_null( )
        => Assert.Throws<ArgumentNullException>( ( ) => new CrawlerJsonPersistenceDescriptor( default! ) );
}