using AngleSharp;
using AngleSharp.Html.Dom;

namespace Earl.Crawler.Middleware.UrlScraping.Scrapers.Tests;

public sealed class AnchorUrlScraperTests
{
    [Fact]
    public async Task Scraper_scrapes_urls_from_anchors( )
    {
        var document = await BrowsingContext.New()
            .OpenAsync(
                response =>
                {
                    response.Address( new Uri( "https://localhost:8080/earl" ) );
                    response.Content( @"<DOCTYPE html>
<html>
<head>
<title>Hello, World!</title>
</head>
<body>
<h1 id=""header"">Hello, World!</h1>
<p>
<a href=""https://localhost:8080/earl"">Earl (localhost)</a>
<a href=""//localhost:8080/earl"">Earl (localhost, no-protocol)</a>
<a href=""https://github.com/cryptoc1/earl"">Earl (GitHub)</a>
<a href="""">Empty</a>
<a href=""#header"">Hashlink (header)</a>
<a href=""https://github.com/cryptoc1/earl#documentation"">Earl Documentation (GitHub Hashlink)</a>
</p>
</body>
</html>" );
                }
            ) as IHtmlDocument;

        var urls = new[]
        {
            new Uri( "https://localhost:8080/earl" ),
            new Uri( "https://github.com/cryptoc1/earl" ),
            new Uri( "https://github.com/cryptoc1/earl#documentation" ),
        };

        var scraper = new AnchorUrlScraper();
        Assert.Collection(
            await scraper.ScrapeAsync( document! ).ToListAsync(),
            urls.Select( url => ( Action<Uri> )( ( Uri scrapedUrl ) => Assert.Equal( url, scrapedUrl ) ) ).ToArray()
        );
    }
}