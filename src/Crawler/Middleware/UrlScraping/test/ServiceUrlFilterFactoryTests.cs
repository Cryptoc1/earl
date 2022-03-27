using AngleSharp.Html.Dom;
using Earl.Crawler.Middleware.UrlScraping.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Middleware.UrlScraping.Tests;

public sealed class ServiceUrlFilterFactoryTests
{
    [Fact]
    public void Factory_creates_filter_using_typed_factory( )
    {
        var serviceProvider = new ServiceCollection()
            .AddTransient<UrlFilterFactory<ServiceUrlFilterDescriptor>, ServiceUrlFilterFactory>()
            .BuildServiceProvider();

        var factory = new UrlFilterFactory( serviceProvider );

        var filter = factory.Create( new ServiceUrlFilterDescriptor( typeof( Filter ) ) );
        Assert.IsType<Filter>( filter );
    }

    private sealed class Filter : IUrlFilter
    {
        public IAsyncEnumerable<Uri> FilterAsync( IAsyncEnumerable<Uri> urls, IHtmlDocument document, CancellationToken cancellation = default ) => throw new NotImplementedException();
    }
}