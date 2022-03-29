using Earl.Crawler.Middleware.Http.Abstractions;
using Microsoft.Extensions.Primitives;

namespace Earl.Crawler.Middleware.Http;

public record HttpResponseMetadata(
    IReadOnlyDictionary<string, StringValues> ContentHeaders,
    TimeSpan Duration,
    IReadOnlyDictionary<string, StringValues> Headers,
    string? ReasonPhrase,
    int StatusCode
) : IHttpResponseMetadata;