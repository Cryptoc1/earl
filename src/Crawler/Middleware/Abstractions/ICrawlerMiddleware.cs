namespace Earl.Crawler.Middleware.Abstractions
{

    /// <summary> Describes a service that may be configured in a pipeline that processes the crawl of a url. </summary>
    /// <see cref="CrawlUrlContext"/>
    public interface ICrawlerMiddleware
    {

        /// <summary> Handle a url crawl for the given <paramref name="context"/>. </summary>
        /// <param name="context"> The <see cref="CrawlUrlContext"/> of the current url crawl. </param>
        /// <param name="next"> A <see cref="CrawlUrlDelegate"/> that continues the url crawl pipeline. </param>
        /// <remarks> Implementers are required to <c>await</c> the <paramref name="next"/> delegate to continue the pipeline. Failure to do so may result in deadlocks or hanging crawls. </remarks>
        Task InvokeAsync( CrawlUrlContext context, CrawlUrlDelegate next );

    }

}
