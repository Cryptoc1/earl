namespace Earl.Crawler.Middleware.Http
{

    public class EarlHttpResponseMessage : HttpResponseMessage
    {

        public TimeSpan TotalDuration { get; set; }

    }

}
