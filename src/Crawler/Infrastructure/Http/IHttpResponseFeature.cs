using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.Extensions.Primitives;

namespace Earl.Crawler.Infrastructure.Http
{

    public interface IHttpResponseFeature
    {

        Stream Body { get; }

        IReadOnlyDictionary<string, StringValues> ContentHeaders { get; }

        IReadOnlyDictionary<string, StringValues> Headers { get; }

        Uri RequestedUrl { get; }

        HttpStatusCode StatusCode { get; }

    }

}
