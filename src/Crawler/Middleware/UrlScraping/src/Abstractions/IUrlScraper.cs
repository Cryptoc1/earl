using AngleSharp.Html.Dom;

namespace Earl.Crawler.Middleware.UrlScraping.Abstractions;

/// <summary> Describes a service that can scrape urls from an <see cref="IHtmlDocument"/>. </summary>
public interface IUrlScraper
{
    /// <summary> Scrape urls from the given <paramref name="document"/>. </summary>
    /// <param name="document"> The document to scrape urls from. </param>
    /// <param name="cancellation"> A token that cancels scraping. </param>
    IAsyncEnumerable<Uri> ScrapeAsync( IHtmlDocument document, CancellationToken cancellation = default );
}