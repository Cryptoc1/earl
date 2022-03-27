using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Middleware.UrlScraping.Abstractions.Configuration;

namespace Earl.Crawler.Middleware.UrlScraping.Configuration.Tests;

public sealed class UrlScraperCrawlerOptionsBuilderExtensionsTests
{
    [Fact]
    public void Use_registers_scraper_middleware_once( )
    {
        var options = new OptionsBuilder()
            .UseUrlScraper()
            .UseUrlScraper()
            .Build();

        Assert.Collection(
            options.Middleware,
            middleware =>
            {
                var descriptor = Assert.IsType<ServiceCrawlerMiddlewareDescriptor>( middleware );
                Assert.Equal( typeof( UrlScraperMiddleware ), descriptor.MiddlewareType );
                Assert.NotNull( descriptor.Options );
            }
        );
    }

    [Fact]
    public void Use_registers_scraper_middleware_with_options_once( )
    {
        var scraperOptions = new UrlScraperOptions( default!, default! );
        var options = new OptionsBuilder()
            .UseUrlScraper()
            .UseUrlScraper( _ => scraperOptions )
            .Build();

        Assert.Collection(
            options.Middleware,
            middleware =>
            {
                var descriptor = Assert.IsType<ServiceCrawlerMiddlewareDescriptor>( middleware );
                Assert.Equal( typeof( UrlScraperMiddleware ), descriptor.MiddlewareType );
                Assert.Equal( scraperOptions, descriptor.Options );
            }
        );
    }

    private sealed class OptionsBuilder : ICrawlerOptionsBuilder
    {
        public IList<CrawlerOptionsBuildAction> BuildActions { get; } = new List<CrawlerOptionsBuildAction>();

        public CrawlerOptions Build( )
        {
            var options = new CrawlerOptions( default, default, default!, default, default, new List<ICrawlerMiddlewareDescriptor>(), default );
            foreach( var action in BuildActions )
            {
                options = action( this, options );
            }

            return options;
        }
    }
}