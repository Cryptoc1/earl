using AngleSharp;
using AngleSharp.Html.Dom;
using Earl.Crawler.Middleware.UrlScraping.Abstractions;
using Earl.Crawler.Middleware.UrlScraping.Abstractions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Middleware.UrlScraping.Tests;

public sealed class UrlFilterInvokerTests
{
    [Fact]
    public async void Invoker_invokes_filters_sequentially( )
    {
        var document = await BrowsingContext.New()
            .OpenAsync( response => response.Address( new Uri( "https://localhost:8080/earl" ) ) ) as IHtmlDocument;

        var serviceProvider = new ServiceCollection()
            .AddTransient<UrlFilterFactory<FilterDescriptor>, FilterFactory>()
            .BuildServiceProvider();

        var factory = new UrlFilterFactory( serviceProvider );
        var invoker = new UrlFilterInvoker( factory );

        int[]? ids = Enumerable.Range( 0, 5 ).ToArray();
        var filters = ids.Select( id => new FilterDescriptor( id ) )
            .ToArray();

        var urls = await invoker.InvokeAsync( filters, document!, AsyncEnumerable.Empty<Uri>() )
            .ToListAsync();

        Assert.Collection(
            urls,
            ids.Select( id => ( Action<Uri> )( ( Uri url ) => Assert.Equal( new Uri( $"https://localhost:8080/{id}" ), url ) ) )
                .ToArray()
        );
    }

    private sealed class FilterDescriptor : IUrlFilterDescriptor
    {
        public int Id { get; }

        public FilterDescriptor( int id )
            => Id = id;
    }

    private sealed class FilterFactory : UrlFilterFactory<FilterDescriptor>
    {
        public override IUrlFilter Create( FilterDescriptor descriptor )
            => new Filter( descriptor.Id );
    }

    private sealed class Filter : IUrlFilter
    {
        private readonly int id;

        public Filter( int id )
            => this.id = id;

        public IAsyncEnumerable<Uri> FilterAsync( IAsyncEnumerable<Uri> urls, IHtmlDocument document, CancellationToken cancellation = default )
            => urls.Append( new Uri( $"https://localhost:8080/{id}" ) );
    }
}