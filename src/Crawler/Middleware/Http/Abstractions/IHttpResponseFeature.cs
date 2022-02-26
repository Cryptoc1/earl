namespace Earl.Crawler.Middleware.Http.Abstractions;

/// <summary> Describes a feature that provides access to the <see cref="HttpResponseMessage"/> of the current url crawl request. </summary>
public interface IHttpResponseFeature
{
    /// <summary> The response to the current url crawl request. </summary>
    HttpResponseMessage Response { get; }
}