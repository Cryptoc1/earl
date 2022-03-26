using AngleSharp.Html.Dom;

namespace Earl.Crawler.Middleware.UrlScraping.Abstractions;

/// <summary> Describes a service that filters urls scraped via <see cref="IUrlScraper.ScrapeAsync(IHtmlDocument, CancellationToken)"/>. </summary>
public interface IUrlFilter
{
    /// <summary> Filters the given <paramref name="urls"/> from the given <paramref name="document"/>. </summary>
    /// <param name="urls"> The urls to filter. </param>
    /// <param name="document"> The document the <paramref name="urls"/> were scraped from. </param>
    /// <param name="cancellation"> A token that cancels filtering. </param>
    /// <returns> The filtered urls. </returns>
    IAsyncEnumerable<Uri> FilterAsync( IAsyncEnumerable<Uri> urls, IHtmlDocument document, CancellationToken cancellation = default );
}