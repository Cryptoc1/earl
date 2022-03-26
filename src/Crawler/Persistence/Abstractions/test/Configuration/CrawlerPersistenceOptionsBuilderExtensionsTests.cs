namespace Earl.Crawler.Persistence.Abstractions.Configuration.Tests;

public sealed class CrawlerPersistenceOptionsBuilderExtensionsTests
{
    [Fact]
    public void Configure_adds_build_action_to_builder( )
    {
        static CrawlerPersistenceOptions action( ICrawlerPersistenceOptionsBuilder builder, CrawlerPersistenceOptions options ) => options;
        var builder = new OptionsBuilder()
            .Configure( action );

        Assert.Collection( builder.BuildActions, a => Assert.Equal( action, a ) );
    }

    private sealed class OptionsBuilder : ICrawlerPersistenceOptionsBuilder
    {
        public IList<CrawlerPersistenceOptionsBuildAction> BuildActions { get; } = new List<CrawlerPersistenceOptionsBuildAction>();

        public CrawlerPersistenceOptions Build( )
        {
            var options = new CrawlerPersistenceOptions( default! );
            foreach( var action in BuildActions )
            {
                options = action( this, options );
            }

            return options;
        }
    }
}