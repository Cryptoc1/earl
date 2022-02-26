namespace Earl.Crawler.Middleware.Http.Abstractions;

/// <summary> Represents a <see cref="HttpRequestMessage"/> executed by the <see cref="IEarlHttpClient"/>. </summary>
public class EarlHttpResponseMessage : HttpResponseMessage
{
    /// <summary> The duration of time spent executing the request. </summary>
    public TimeSpan TotalDuration { get; set; }
}