namespace Earl.Crawler.Middleware.Abstractions
{

    /// <summary> Describes a service that can invoke a pipeline of <see cref="ICrawlerMiddleware"/>. </summary>
    public interface ICrawlerMiddlewareInvoker
    {

        /// <summary> Invokes a pipeline of configured <see cref="ICrawlerMiddleware"/> for the given <paramref name="context"/>. </summary>
        /// <param name="context"> A <see cref="CrawlUrlContext"/> representing a url crawl to invoke the middleware pipeline upon. </param>
        /// <returns> A task that represents the completion of the middleware pipeline's invocation. </returns>
        Task InvokeAsync( CrawlUrlContext context );

    }

}
