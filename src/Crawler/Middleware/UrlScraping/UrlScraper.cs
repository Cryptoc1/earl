using System.Runtime.CompilerServices;
using AngleSharp.Html.Dom;
using Earl.Crawler.Middleware.UrlScraping.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Middleware.UrlScraping;

/// <summary> Default implementation of <see cref="IUrlScraper"/>. </summary>
public class UrlScraper : IUrlScraper
{
    #region Fields
    private readonly IServiceProvider serviceProvider;
    #endregion

    public UrlScraper( IServiceProvider serviceProvider )
        => this.serviceProvider = serviceProvider;

    /// <inheritdoc/>
    public async IAsyncEnumerable<Uri> ScrapeAsync( IHtmlDocument document, Uri baseUrl, [EnumeratorCancellation] CancellationToken cancellation = default )
    {
        ArgumentNullException.ThrowIfNull( document );
        ArgumentNullException.ThrowIfNull( baseUrl );

        var urls = document.QuerySelectorAll( "a:not([href=\"\"])" )
            .ToAsyncEnumerable()
            .Where( element => element is IHtmlAnchorElement )
            .Select( anchor => anchor.GetAttribute( "href" ) )

            // only urls on the same domain
            .Where( href => href?.StartsWith( baseUrl.AbsoluteUri ) is true || href?.StartsWith( '/' ) is true )

            .Distinct( StringComparer.OrdinalIgnoreCase )
            .Select(
                href => Uri.TryCreate( href, UriKind.Absolute, out var url )
                    || Uri.TryCreate( baseUrl.AbsoluteUri + href!.TrimStart( '/' ), UriKind.Absolute, out url )
                        ? url : null
            )
            .Where( url => url is not null )
            .Select( url => url! )

            // ignore '{baseUrl}/#'
            .Where( url => !( !string.IsNullOrEmpty( url.Fragment ) && url.AbsolutePath is "/" ) );

        await foreach( var url in FilterAsync( urls, cancellation ) )
        {
            yield return url;
        }
    }

    private IAsyncEnumerable<Uri> FilterAsync( IAsyncEnumerable<Uri> urls, CancellationToken cancellation )
    {
        var filters = serviceProvider.GetService<IEnumerable<IUrlScraperFilter>>();
        if( filters?.Any() is true )
        {
            foreach( var filter in filters )
            {
                urls = filter.FilterAsync( urls, cancellation );
            }
        }

        return urls;
    }
}