using System;
using System.Net.Http;

namespace Earl.Crawler.Infrastructure.Http
{

    public class EarlHttpResponseMessage : HttpResponseMessage
    {

        public TimeSpan TotalDuration { get; set; }

    }

}
