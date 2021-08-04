using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;

namespace Earl.Crawler.Infrastructure.UrlScraper.Abstractions
{

    public interface IUrlScraper
    {

        Task<IEnumerable<Uri>> ScrapeAsync( IHtmlDocument document, Uri baseUrl, CancellationToken cancellation = default );

    }

}
