using AngleSharp.Html.Dom;

namespace Earl.Crawler.Middleware.UrlScraping.Abstractions
{

    /// <summary> Describes a service that can scrape urls from an <see cref="IHtmlDocument"/>. </summary>
    public interface IUrlScraper
    {

        /// <summary> Scrape urls from the given <paramref name="document"/>, excluding urls with the given <paramref name="baseUrl"/>. </summary>
        /// <param name="document"> The document to scrape urls from. </param>
        /// <param name="baseUrl"> The base (authority) url to be ignored. </param>
        /// <returns> Scraped urls to be queued for further crawling. </returns>
        Task<IEnumerable<Uri>> ScrapeAsync( IHtmlDocument document, Uri baseUrl, CancellationToken cancellation = default );

    }

}
