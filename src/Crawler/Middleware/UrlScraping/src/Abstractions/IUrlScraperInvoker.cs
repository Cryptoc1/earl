using AngleSharp.Html.Dom;
using Earl.Crawler.Middleware.UrlScraping.Abstractions.Configuration;

namespace Earl.Crawler.Middleware.UrlScraping.Abstractions;

/// <summary> Describes a service that can invoke <see cref="IUrlScraper"/>s. </summary>
public interface IUrlScraperInvoker
{
    /// <summary> Invokes the configured <see cref="IUrlScraper"/>s upon the <paramref name="document"/>. </summary>
    /// <param name="scrapers"> The configuration of <see cref="IUrlScraper"/>s to invoke. </param>
    /// <param name="document"> The document to invoke the scrapers on. </param>
    /// <param name="cancellation"> A token that cancels the operation. </param>
    IAsyncEnumerable<Uri> InvokeAsync( IReadOnlyList<IUrlScraperDescriptor> scrapers, IHtmlDocument document, CancellationToken cancellation = default );
}