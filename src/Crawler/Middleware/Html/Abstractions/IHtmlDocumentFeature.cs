using AngleSharp.Html.Dom;
using Earl.Crawler.Middleware.Abstractions;

namespace Earl.Crawler.Middleware.Html.Abstractions;

/// <summary> Describes a feature that provides access to an <see cref="IHtmlDocument"/> of the current request. </summary>
public interface IHtmlDocumentFeature
{
    /// <summary> An <see cref="IHtmlDocument"/> of the requested <see cref="CrawlUrlContext.Url"/>. </summary>
    IHtmlDocument Document { get; }
}