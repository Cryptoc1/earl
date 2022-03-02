using AngleSharp.Html.Dom;
using Earl.Crawler.Middleware.UrlScraping.Abstractions;
using Earl.Crawler.Middleware.UrlScraping.Abstractions.Configuration;

namespace Earl.Crawler.Middleware.UrlScraping;

/// <summary> Default implementation of <see cref="IUrlScraper"/>. </summary>
public class UrlScraper : IUrlScraper
{
    #region Fields
    private readonly IUrlFilterInvoker filterInvoker;
    #endregion

    public UrlScraper( IUrlFilterInvoker filterInvoker )
        => this.filterInvoker = filterInvoker;

    /// <inheritdoc/>
    public IAsyncEnumerable<Uri> ScrapeAsync( IHtmlDocument document, UrlScraperOptions options, CancellationToken cancellation = default )
    {
        ArgumentNullException.ThrowIfNull( document );

        var urls = document.QuerySelectorAll( "a:not([href=\"\"])" )
            .Where( element => element is IHtmlAnchorElement )
            .Select( anchor => anchor.GetAttribute( "href" ) )

            // ignore fragments
            .Where( href => !string.IsNullOrWhiteSpace( href ) && href?.StartsWith( '#' ) is false )

            // Force protocol
            .Select( href => href!.StartsWith( "//" ) ? document.Location.Protocol + href : href )
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

        return filterInvoker.InvokeAsync( urls, document, options, cancellation );
    }
}