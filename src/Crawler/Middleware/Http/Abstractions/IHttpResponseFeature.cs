using System.Net;
using Microsoft.Extensions.Primitives;

namespace Earl.Crawler.Middleware.Http.Abstractions
{

    public interface IHttpResponseFeature
    {

        MemoryStream Body { get; }

        IReadOnlyDictionary<string, StringValues> ContentHeaders { get; }

        IReadOnlyDictionary<string, StringValues> Headers { get; }

        Uri RequestedUrl { get; }

        IHttpStatistics Statistics { get; }

        HttpStatusCode StatusCode { get; }

    }

}
