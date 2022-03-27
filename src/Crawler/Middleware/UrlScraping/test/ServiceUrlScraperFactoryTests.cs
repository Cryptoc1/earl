using AngleSharp.Html.Dom;
using Earl.Crawler.Middleware.UrlScraping.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Middleware.UrlScraping.Tests;

public sealed class ServiceUrlScraperFactoryTests
{
    [Fact]
    public void Factory_creates_scraper( )
    {
        var serviceProvider = new ServiceCollection().BuildServiceProvider();

        var factory = new ServiceUrlScraperFactory( serviceProvider );
        var descriptor = new ServiceUrlScraperDescriptor( typeof( Scraper ) );

        var filter = factory.Create( descriptor );
        Assert.IsType<Scraper>( filter );
    }

    [Fact]
    public void Factory_creates_scraper_with_dependency( )
    {
        var serviceProvider = new ServiceCollection()
            .AddTransient<ScraperDependency>()
            .BuildServiceProvider();

        var factory = new ServiceUrlScraperFactory( serviceProvider );
        var descriptor = new ServiceUrlScraperDescriptor( typeof( ScraperWithDependency ) );

        var scraper = factory.Create( descriptor );
        var typedScraper = Assert.IsType<ScraperWithDependency>( scraper );
        Assert.NotNull( typedScraper.Dependency );
    }

    private sealed class Scraper : IUrlScraper
    {
        public IAsyncEnumerable<Uri> ScrapeAsync( IHtmlDocument document, CancellationToken cancellation = default ) => throw new NotImplementedException();
    }

    private sealed class ScraperWithDependency : IUrlScraper
    {
        public ScraperDependency Dependency { get; }

        public ScraperWithDependency( ScraperDependency dependency )
            => Dependency = dependency;

        public IAsyncEnumerable<Uri> ScrapeAsync( IHtmlDocument document, CancellationToken cancellation = default ) => throw new NotImplementedException();
    }

    private sealed class ScraperDependency
    {
    }
}