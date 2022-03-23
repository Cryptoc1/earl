using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Earl.Crawler.Middleware.UrlScraping.Abstractions;

namespace Earl.Crawler.Middleware.UrlScraping.Filters;

/// <summary> An <see cref="IUrlFilter"/> that filters scraped urls to those that match the <see cref="IDocument.Origin"/>. </summary>
public class SameOriginUrlFilter : IUrlFilter
{
    /// <inheritdoc/>
    public IAsyncEnumerable<Uri> FilterAsync( IAsyncEnumerable<Uri> urls, IHtmlDocument document, CancellationToken cancellation = default )
    {
        ArgumentNullException.ThrowIfNull( urls );
        ArgumentNullException.ThrowIfNull( document );
        return urls.Where( url => url.AbsoluteUri.StartsWith( document.Origin! ) );
    }
}