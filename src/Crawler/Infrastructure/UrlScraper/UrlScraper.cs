using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using Earl.Crawler.Infrastructure.UrlScraper.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Infrastructure.UrlScraper
{

    public class UrlScraper : IUrlScraper
    {
        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion

        public UrlScraper( IServiceProvider serviceProvider )
            => this.serviceProvider = serviceProvider;

        public async Task<IEnumerable<Uri>> ScrapeAsync( IHtmlDocument document, Uri baseUrl, CancellationToken cancellation = default )
        {
            var urls = document.QuerySelectorAll( "a:not([href=\"\"])" )
                ?.Select( anchor => anchor.GetAttribute( "href" ) )
                .Where( href => !string.IsNullOrWhiteSpace( href ) )

                // only urls on the same domain
                .Where( href => href.StartsWith( baseUrl.AbsoluteUri ) || href.StartsWith( '/' ) )

                .Distinct( StringComparer.OrdinalIgnoreCase )
                .Select(
                    href => Uri.TryCreate( href, UriKind.Absolute, out var url )
                        || Uri.TryCreate( baseUrl.AbsoluteUri + href.TrimStart( '/' ), UriKind.Absolute, out url )
                            ? url : null
                )
                .Where( url => url is not null )
                .Select( url => url! )

                // ignore '{baseUrl}/#'
                .Where( url => !( !string.IsNullOrEmpty( url.Fragment ) && url.AbsolutePath == "/" ) )
                    ?? Enumerable.Empty<Uri>();

            var filters = serviceProvider.GetService<IEnumerable<IUrlScraperFilter>>();
            if( filters?.Any() is true )
            {
                foreach( var filter in filters )
                {
                    urls = await filter.FilterAsync( urls, cancellation );
                }
            }

            return urls;
        }

    }

}
