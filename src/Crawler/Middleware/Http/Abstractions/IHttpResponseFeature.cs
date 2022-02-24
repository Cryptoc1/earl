namespace Earl.Crawler.Middleware.Http.Abstractions;

public interface IHttpResponseFeature
{
    HttpResponseMessage Response { get; }
}