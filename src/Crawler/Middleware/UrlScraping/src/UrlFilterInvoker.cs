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
    public IAsyncEnumerable<Uri> InvokeAsync( IReadOnlyList<IUrlFilterDescriptor> filters, IHtmlDocument document, IAsyncEnumerable<Uri> urls, CancellationToken cancellation = default )
    {
        ArgumentNullException.ThrowIfNull( filters );
        ArgumentNullException.ThrowIfNull( document );
        ArgumentNullException.ThrowIfNull( urls );

        foreach( var filter in filters.Select( filterFactory.Create ) )
        {
            urls = filter.FilterAsync( urls, document, cancellation );
        }

        return urls;
    }
}