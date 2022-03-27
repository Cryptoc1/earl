using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Earl.Crawler.Middleware.UrlScraping.Abstractions;

namespace Earl.Crawler.Middleware.UrlScraping.Scrapers;

/// <summary> An <see cref="IUrlScraper"/> that scrapes urls from HTML anchor (<![CDATA[<a>]]>) elements. </summary>
/// <remarks> Ignores empty and relative fragment anchors. </remarks>
public sealed class AnchorUrlScraper : IUrlScraper
{
    /// <inheritdoc/>
    public IAsyncEnumerable<Uri> ScrapeAsync( IHtmlDocument document, CancellationToken cancellation = default )
    {
        ArgumentNullException.ThrowIfNull( document );
        return document.QuerySelectorAll<IHtmlAnchorElement>( "a:not([href=\"\"])" )

            // ignore relative fragments
            .Where( anchor => string.IsNullOrWhiteSpace( anchor.Hash ) || !anchor.Href.Equals( document.Location.Href + anchor.Hash, StringComparison.OrdinalIgnoreCase ) )

            // Force protocol
            .Select( anchor => anchor.Href )
            .Distinct( StringComparer.OrdinalIgnoreCase )
            .Select( href => Uri.TryCreate( href, UriKind.Absolute, out var url ) ? url : null )
            .Where( url => url is not null )
            .Select( url => url! )
            .ToAsyncEnumerable();
    }
}