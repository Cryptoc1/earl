using AngleSharp.Html.Dom;
using Earl.Crawler.Abstractions;
using Earl.Crawler.Middleware.UrlScraping.Abstractions;

namespace Earl.Crawler.Middleware.UrlScraping.Filters;

/// <summary> An <see cref="IUrlFilter"/> that utilitizes <see cref="UriComparer.OrdinalIgnoreCase"/> to filter distinct urls. </summary>
/// <remarks> For best use, ensure this filter is registered before all other filters. </remarks>
public sealed class DistinctUrlFilter : IUrlFilter
{
    /// <inheritdoc/>
    public IAsyncEnumerable<Uri> FilterAsync( IAsyncEnumerable<Uri> urls, IHtmlDocument document, CancellationToken cancellation = default )
    {
        ArgumentNullException.ThrowIfNull( urls );
        return urls.Distinct( UriComparer.OrdinalIgnoreCase );
    }
}