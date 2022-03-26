using AngleSharp.Html.Dom;

namespace Earl.Crawler.Middleware.UrlScraping.Abstractions.Configuration.Tests;

public sealed class UrlScraperOptionsExtensionsTests
{
    [Fact]
    public void Options_registers_filter( )
    {
        var options = new UrlScraperOptions( new List<IUrlFilterDescriptor>(), default! )
            .WithFilter<TestFilter>();

        Assert.Collection(
            options.Filters,
            descriptor => Assert.Equal(
                typeof( TestFilter ),
                Assert.IsType<ServiceUrlFilterDescriptor>( descriptor ).FilterType
            )
        );
    }

    [Fact]
    public void Options_registers_scraper( )
    {
        var options = new UrlScraperOptions( default!, new List<IUrlScraperDescriptor>() )
            .WithScraper<TestScraper>();

        Assert.Collection(
            options.Scrapers,
            descriptor => Assert.Equal(
                typeof( TestScraper ),
                Assert.IsType<ServiceUrlScraperDescriptor>( descriptor ).ScraperType
            )
        );
    }

    private sealed class TestFilter : IUrlFilter
    {
        public IAsyncEnumerable<Uri> FilterAsync( IAsyncEnumerable<Uri> urls, IHtmlDocument document, CancellationToken cancellation = default ) => throw new NotImplementedException();
    }

    private sealed class TestScraper : IUrlScraper
    {
        public IAsyncEnumerable<Uri> ScrapeAsync( IHtmlDocument document, CancellationToken cancellation = default ) => throw new NotImplementedException();
    }
}