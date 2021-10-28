using Microsoft.Extensions.Primitives;

namespace Earl.Crawler.Middleware.Http.Abstractions
{

    public interface IHttpResponseMetadata
    {

        IReadOnlyDictionary<string, StringValues> ContentHeaders { get; }

        TimeSpan Duration { get; }

        IReadOnlyDictionary<string, StringValues> Headers { get; }

        string? ReasonPhrase { get; }

        int StatusCode { get; }

    }

}
