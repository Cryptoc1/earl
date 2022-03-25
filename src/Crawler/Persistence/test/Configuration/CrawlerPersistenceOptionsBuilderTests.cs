namespace Earl.Crawler.Persistence.Configuration.Tests;

public sealed class CrawlerPersistenceOptionsBuilderTests
{
    [Fact]
    public void Builder_builds_options_with_build_actions( )
    {
        var builder = new CrawlerPersistenceOptionsBuilder();

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
}