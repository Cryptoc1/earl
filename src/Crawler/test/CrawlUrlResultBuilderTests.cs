namespace Earl.Crawler.Tests;

public sealed class CrawlUrlResultBuilderTests
{
    [Fact]
    public void Builder_uses_display_name_in_result( )
    {
        var builder = new CrawlUrlResultBuilder( default!, displayName: "Test" );

        var result = builder.Build();
        Assert.Equal( "Test", result.DisplayName );
    }

    [Fact]
    public void Builder_uses_metadata_in_result( )
    {
        var builder = new CrawlUrlResultBuilder( default!, metadata: new object[] { "Test" } );

        var result = builder.Build();
        Assert.Collection(
            result.Metadata,
            data => Assert.Equal( "Test", data )
        );
    }
}