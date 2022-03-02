using AngleSharp.Html.Dom;
using Earl.Crawler.Middleware.UrlScraping.Abstractions.Configuration;

namespace Earl.Crawler.Middleware.UrlScraping.Abstractions;

/// <summary> Describes a service that can scrape urls from an <see cref="IHtmlDocument"/>. </summary>
public interface IUrlScraper
{
    /// <summary> Scrape urls from the given <paramref name="document"/>. </summary>
    /// <param name="document"> The document to scrape urls from. </param>
    /// <param name="options"> The options for scraping. </param>
    /// <param name="cancellation"> A token that cancels scraping. </param>
    /// <returns> Scraped urls to be queued for further crawling. </returns>
    IAsyncEnumerable<Uri> ScrapeAsync( IHtmlDocument document, UrlScraperOptions options, CancellationToken cancellation = default );
}