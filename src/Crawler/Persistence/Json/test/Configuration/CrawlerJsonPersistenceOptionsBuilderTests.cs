using Earl.Crawler.Persistence.Json.Serialization;

namespace Earl.Crawler.Persistence.Json.Configuration.Tests;

public sealed class CrawlerJsonPersistenceOptionsBuilderTests
{
    [Fact]
    public void Builder_builds_options_with_build_actions( )
    {
        var builder = CrawlerJsonPersistenceOptionsBuilder.CreateDefault()
            .Destination( "." );

        bool built = false;
        builder.BuildActions.Add(
            ( _, options ) =>
            {
                built = true;
                return options;
            }
        );

        builder.Build();
        Assert.True( built );
    }

    [Fact]
    public void Builder_throws_exception_when_destination_is_null( )
    {
        var builder = CrawlerJsonPersistenceOptionsBuilder.CreateDefault();
        Assert.Throws<ArgumentNullException>( ( ) => builder.Build() );
    }

    [Fact]
    public void Builder_uses_destination( )
    {
        var options = CrawlerJsonPersistenceOptionsBuilder.CreateDefault()
            .Destination( "TEST" )
            .Build();

        Assert.Equal( "TEST", options.Destination );
    }

    [Fact]
    public void Default_builder_adds_result_metadata_converter( )
    {
        var options = CrawlerJsonPersistenceOptionsBuilder.CreateDefault()
            .Destination( "." )
            .Build();

        Assert.Contains(
            options.Serialization.Converters,
            converter => converter is ResultMetadataConverter
        );
    }
}