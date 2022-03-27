using AngleSharp;
using AngleSharp.Html.Dom;
using Earl.Crawler.Middleware.Abstractions;
using Earl.Crawler.Middleware.Html.Abstractions;
using Earl.Crawler.Middleware.UrlScraping.Abstractions.Configuration;
using Earl.Crawler.Middleware.UrlScraping.Scrapers;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Middleware.UrlScraping.Tests;

public sealed class UrlScraperMiddlewareTests
{
    [Fact]
    public async Task Middleware_continues_without_document_feature( )
    {
        await using var features = new CrawlerFeatureCollection();
        var context = new CrawlUrlContext(
            new( default!, CancellationToken.None, default!, default!, default!, default! ),
            features,
            default!,
            default!,
            default!
        );

        var options = new UrlScraperOptions( default!, default! );
        var middleware = new UrlScraperMiddleware( default!, options, default! );

        bool continued = false;
        await middleware.InvokeAsync(
            context,
            _ =>
            {
                continued = true;
                return Task.CompletedTask;
            }
        );

        Assert.True( continued );
    }

    [Fact]
    public async Task Middleware_enqueues_scraped_urls( )
    {
        var serviceProvider = new ServiceCollection()
            .AddTransient<UrlFilterFactory<ServiceUrlFilterDescriptor>, ServiceUrlFilterFactory>()
            .AddTransient<UrlScraperFactory<ServiceUrlScraperDescriptor>, ServiceUrlScraperFactory>()
            .BuildServiceProvider();

        await using var features = new CrawlerFeatureCollection();
        var context = new CrawlUrlContext(
            new( default!, CancellationToken.None, default!, default!, default!, new() ),
            features,
            default!,
            default!,
            default!
        );

        using var document = await BrowsingContext.New()
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
</p>
</body>
</html>" );
                }
            ) as IHtmlDocument;

        features.Set<IHtmlDocumentFeature>( new HtmlDocumentFeature( document! ) );

        var options = new UrlScraperOptions(
            new List<IUrlFilterDescriptor>(),
            new List<IUrlScraperDescriptor>() { new ServiceUrlScraperDescriptor( typeof( AnchorUrlScraper ) ) }
        );

        var middleware = new UrlScraperMiddleware(
            new UrlFilterInvoker( new UrlFilterFactory( serviceProvider ) ),
            options,
            new UrlScraperInvoker( new UrlScraperFactory( serviceProvider ) )
        );

        await middleware.InvokeAsync( context, _ => Task.CompletedTask );

        Assert.Collection(
            context.CrawlContext.UrlQueue,
            url => Assert.Equal( new Uri( "https://localhost:8080/earl" ), url )
        );
    }

    private sealed class HtmlDocumentFeature : IHtmlDocumentFeature
    {
        public IHtmlDocument Document { get; }

        public HtmlDocumentFeature( IHtmlDocument document ) => Document = document;
    }
}