using AngleSharp.Html.Dom;
using Earl.Crawler.Middleware.UrlScraping.Abstractions.Configuration;

namespace Earl.Crawler.Middleware.UrlScraping.Abstractions;

/// <summary> Describes a service that can invoke <see cref="IUrlFilter"/>s. </summary>
public interface IUrlFilterInvoker
{
    /// <summary> Invokes the given <paramref name="filters"/> upon the <paramref name="urls"/> scraped from <paramref name="document"/>. </summary>
    /// <param name="filters"> The configuration of <see cref="IUrlFilter"/>s to invoke. </param>
    /// <param name="document"> The document to invoke the filters on. </param>
    /// <param name="urls"> The urls to invoke the filters on. </param>
    /// <param name="cancellation"> A token that cancels the operation. </param>
    IAsyncEnumerable<Uri> InvokeAsync( IReadOnlyList<IUrlFilterDescriptor> filters, IHtmlDocument document, IAsyncEnumerable<Uri> urls, CancellationToken cancellation = default );
}