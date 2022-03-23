using AngleSharp.Html.Dom;
using Earl.Crawler.Middleware.UrlScraping.Abstractions;
using Earl.Crawler.Middleware.UrlScraping.Abstractions.Configuration;

namespace Earl.Crawler.Middleware.UrlScraping;

/// <summary> Default implementation of <see cref="IUrlFilterInvoker"/>. </summary>
public sealed class UrlFilterInvoker : IUrlFilterInvoker
{
    private readonly IUrlFilterFactory filterFactory;

    public UrlFilterInvoker( IUrlFilterFactory filterFactory )
        => this.filterFactory = filterFactory;

    /// <inheritdoc/>
    public IAsyncEnumerable<Uri> InvokeAsync( IAsyncEnumerable<Uri> urls, IHtmlDocument document, UrlScraperOptions options, CancellationToken cancellation = default )
    {
        ArgumentNullException.ThrowIfNull( urls );
        ArgumentNullException.ThrowIfNull( document );
        ArgumentNullException.ThrowIfNull( options );

        var filters = options.Filters.Select( filterFactory.Create );
        foreach( var filter in filters )
        {
            urls = filter.FilterAsync( urls, document, cancellation );
        }

        return urls;
    }
}