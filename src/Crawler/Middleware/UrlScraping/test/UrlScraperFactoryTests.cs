using AngleSharp.Html.Dom;
using Earl.Crawler.Middleware.UrlScraping.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Middleware.UrlScraping.Tests;

public sealed class UrlScraperFactoryTests
{
    [Fact]
    public void Factory_creates_scraper_using_typed_factory( )
    {
        var serviceProvider = new ServiceCollection()
            .AddTransient<UrlScraperFactory<ServiceUrlScraperDescriptor>, ServiceUrlScraperFactory>()
            .BuildServiceProvider();

        var factory = new UrlScraperFactory( serviceProvider );

        var scraper = factory.Create( new ServiceUrlScraperDescriptor( typeof( Scraper ) ) );
        Assert.IsType<Scraper>( scraper );
    }

    private sealed class Scraper : IUrlScraper
    {
        public IAsyncEnumerable<Uri> ScrapeAsync( IHtmlDocument document, CancellationToken cancellation = default ) => throw new NotImplementedException();
    }
}