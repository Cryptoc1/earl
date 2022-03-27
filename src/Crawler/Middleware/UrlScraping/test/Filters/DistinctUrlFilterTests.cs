namespace Earl.Crawler.Middleware.UrlScraping.Filters.Tests;

public sealed class DistinctUrlFilterTests
{
    [Fact]
    public async Task Filter_filters_urls_ignoring_case( )
    {
        var earlUrl = new Uri( "https://localhost:8080/earl" );
        var urls = new[]
        {
            earlUrl,
            new Uri("https://localhost:8080/earl"),
            new Uri("https://localhost:8080/Earl"),
            new Uri("HTTPS://LOCALHOST:8080/EARL"),
            new Uri("HTTPS://LOCALHOST:8080/earl"),
            new Uri("https://LOCALHOST:8080/earl"),
            new Uri("https://LOCALHOST:8080/EaRl"),
        }.ToAsyncEnumerable();

        var filter = new DistinctUrlFilter();
        Assert.Collection(
            await filter.FilterAsync( urls, default! ).ToListAsync(),
            url => Assert.Equal( earlUrl, url )
        );
    }
}