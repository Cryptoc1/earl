using AngleSharp.Html.Dom;

namespace Earl.Crawler.Infrastructure.Html
{

    /// <summary> Describes a feature that provides access to an <see cref="IHtmlDocument"/> of the current request. </summary>
    public interface IHtmlDocumentFeature
    {

        IHtmlDocument Document { get; }

    }

}
