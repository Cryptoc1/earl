using AngleSharp;
using AngleSharp.Html.Dom;
using Earl.Crawler.Middleware.UrlScraping.Abstractions;
using Earl.Crawler.Middleware.UrlScraping.Abstractions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Middleware.UrlScraping.Tests;

public sealed class UrlScraperInvokerTests
{
    [Fact]
    public async void Invoker_invokes_scrapers_sequentially( )
    {
        var document = await BrowsingContext.New()
            .OpenAsync( response => response.Address( new Uri( "https://localhost:8080/earl" ) ) ) as IHtmlDocument;

        var serviceProvider = new ServiceCollection()
            .AddTransient<UrlScraperFactory<ScraperDescriptor>, ScraperFactory>()
            .BuildServiceProvider();

        var factory = new UrlScraperFactory( serviceProvider );
        var invoker = new UrlScraperInvoker( factory );

        int[]? ids = Enumerable.Range( 0, 5 ).ToArray();
        var scrapers = ids.Select( id => new ScraperDescriptor( id ) )
            .ToArray();

        var urls = await invoker.InvokeAsync( scrapers, document! )
            .ToListAsync();

        Assert.Collection(
            urls,
            ids.Select( id => ( Action<Uri> )( ( Uri url ) => Assert.Equal( new Uri( $"https://localhost:8080/{id}" ), url ) ) )
                .ToArray()
        );
    }

    private sealed class ScraperDescriptor : IUrlScraperDescriptor
    {
        public int Id { get; }

        public ScraperDescriptor( int id )
            => Id = id;
    }

    private sealed class ScraperFactory : UrlScraperFactory<ScraperDescriptor>
    {
        public override IUrlScraper Create( ScraperDescriptor descriptor )
            => new Scraper( descriptor.Id );
    }

    private sealed class Scraper : IUrlScraper
    {
        private readonly int id;

        public Scraper( int id )
            => this.id = id;

        public IAsyncEnumerable<Uri> ScrapeAsync( IHtmlDocument document, CancellationToken cancellation = default )
            => AsyncEnumerable.Empty<Uri>()
                .Append( new Uri( $"https://localhost:8080/{id}" ) );
    }
}