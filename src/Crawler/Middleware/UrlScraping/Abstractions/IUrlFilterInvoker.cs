using AngleSharp.Html.Dom;
using Earl.Crawler.Middleware.UrlScraping.Abstractions.Configuration;

namespace Earl.Crawler.Middleware.UrlScraping.Abstractions;

/// <summary> Describes a service that can invoke <see cref="IUrlFilter"/>s. </summary>
public interface IUrlFilterInvoker
{
    /// <summary> Invokes the configured <see cref="IUrlFilter"/> for the given <paramref name="options"/>. </summary>
    /// <param name="urls"> The urls to invoke the filters on. </param>
    /// <param name="document"> The document to invoke the filters on. </param>
    /// <param name="options"> The <see cref="UrlScraperOptions"/> descibing the filters to use. </param>
    /// <param name="cancellation"> A token that cancels the operation. </param>
    IAsyncEnumerable<Uri> InvokeAsync( IAsyncEnumerable<Uri> urls, IHtmlDocument document, UrlScraperOptions options, CancellationToken cancellation = default );
}