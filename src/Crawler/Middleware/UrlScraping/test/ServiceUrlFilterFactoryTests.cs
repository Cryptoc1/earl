using AngleSharp.Html.Dom;
using Earl.Crawler.Middleware.UrlScraping.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Middleware.UrlScraping.Tests;

public sealed class ServiceUrlFilterFactoryTests
{
    [Fact]
    public void Factory_creates_filter( )
    {
        var serviceProvider = new ServiceCollection().BuildServiceProvider();

        var factory = new ServiceUrlFilterFactory( serviceProvider );
        var descriptor = new ServiceUrlFilterDescriptor( typeof( Filter ) );

        var filter = factory.Create( descriptor );
        Assert.IsType<Filter>( filter );
    }

    [Fact]
    public void Factory_creates_filter_with_dependency( )
    {
        var serviceProvider = new ServiceCollection()
            .AddTransient<FilterDependency>()
            .BuildServiceProvider();

        var factory = new ServiceUrlFilterFactory( serviceProvider );
        var descriptor = new ServiceUrlFilterDescriptor( typeof( FilterWithDependency ) );

        var filter = factory.Create( descriptor );
        var typedFilter = Assert.IsType<FilterWithDependency>( filter );
        Assert.NotNull( typedFilter.Dependency );
    }

    private sealed class Filter : IUrlFilter
    {
        public IAsyncEnumerable<Uri> FilterAsync( IAsyncEnumerable<Uri> urls, IHtmlDocument document, CancellationToken cancellation = default ) => throw new NotImplementedException();
    }

    private sealed class FilterWithDependency : IUrlFilter
    {
        public FilterDependency Dependency { get; }

        public FilterWithDependency( FilterDependency dependency )
            => Dependency = dependency;

        public IAsyncEnumerable<Uri> FilterAsync( IAsyncEnumerable<Uri> urls, IHtmlDocument document, CancellationToken cancellation = default ) => throw new NotImplementedException();
    }

    private sealed class FilterDependency
    {
    }
}