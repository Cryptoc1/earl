namespace Earl.Crawler.Middleware.Http.Abstractions;

/// <summary> Describes a typed <see cref="HttpClient"/> that can make HTTP request during a url crawl. </summary>
public interface IEarlHttpClient
{
    /// <summary> Execute a GET request. </summary>
    /// <param name="url"> The url to request. </param>
    /// <param name="cancellation"> A token that cancels the request. </param>
    Task<EarlHttpResponseMessage> GetAsync( Uri url, CancellationToken cancellation = default );
}