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

            // ignore fragments
            .Where( anchor => !string.IsNullOrWhiteSpace( anchor.Href ) && anchor.Href?.StartsWith( '#' ) is false )

            // Force protocol
            .Select( anchor => anchor.Href.StartsWith( "//" ) ? document.Location.Protocol + anchor.Href : anchor.Href )
            .Distinct( StringComparer.OrdinalIgnoreCase )

            // try parse as Uri
            .Select(
                href => Uri.TryCreate( href, UriKind.Absolute, out var url )
                    || Uri.TryCreate( $"{document.Origin?.TrimEnd( '/' )}/{href.TrimStart( '/' )}", UriKind.Absolute, out url )
                    || Uri.TryCreate( href, UriKind.RelativeOrAbsolute, out url ) ? url : null
            )
            .Where( url => url is not null )
            .Select( url => url! )
            .ToAsyncEnumerable();
    }
}