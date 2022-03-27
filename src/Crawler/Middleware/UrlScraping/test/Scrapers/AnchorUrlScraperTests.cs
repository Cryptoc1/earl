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
<h1>Hello, World!</h1>
<p>
<a href=""https://localhost:8080/earl"">Earl (localhost)</a>
<a href=""https://github.com/cryptoc1/earl"">Earl (GitHub)</a>
</p>
</body>
</html>" );
                }
            ) as IHtmlDocument;

        var urls = new[]
        {
            new Uri( "https://localhost:8080/earl" ),
            new Uri( "https://github.com/cryptoc1/earl" ),
        };

        var scraper = new AnchorUrlScraper();
        Assert.Collection(
            await scraper.ScrapeAsync( document! ).ToListAsync(),
            urls.Select( url => ( Action<Uri> )( ( Uri scrapedUrl ) => Assert.Equal( url, scrapedUrl ) ) ).ToArray()
        );
    }
}