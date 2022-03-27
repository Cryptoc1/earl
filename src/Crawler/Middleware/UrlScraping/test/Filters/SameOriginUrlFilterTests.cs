using AngleSharp;
using AngleSharp.Html.Dom;

namespace Earl.Crawler.Middleware.UrlScraping.Filters.Tests;

public sealed class SameOriginUrlFilterTests
{
    [Fact]
    public async Task Filter_filters_urls_by_document_origin( )
    {
        var document = await BrowsingContext.New()
            .OpenAsync( response => response.Address( new Uri( "https://localhost:8080/earl" ) ) ) as IHtmlDocument;

        var earlUrl = new Uri( "https://localhost:8080/earl" );
        var urls = new[]
        {
            earlUrl,
            new Uri( "http://github.com/cryptoc1/earl" ),
        }.ToAsyncEnumerable();

        var filter = new SameOriginUrlFilter();
        Assert.Collection(
            await filter.FilterAsync( urls, document! ).ToListAsync(),
            url => Assert.Equal( earlUrl, url )
        );
    }
}