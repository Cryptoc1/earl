using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Middleware.Abstractions;

namespace Earl.Crawler.Middleware;

/// <summary> Represents an <see cref="ICrawlerMiddlewareDescriptor"/> for a delegate method. </summary>
public class DelegateCrawlerMiddlewareDescriptor : ICrawlerMiddlewareDescriptor
{
    #region Properties

    /// <summary> The delegate method representing the <see cref="ICrawlerMiddleware.InvokeAsync(CrawlUrlContext, CrawlUrlDelegate)"/> body. </summary>
    public Func<CrawlUrlContext, CrawlUrlDelegate, Task> Middleware { get; }
    #endregion

    public DelegateCrawlerMiddlewareDescriptor( Func<CrawlUrlContext, CrawlUrlDelegate, Task> middleware )
        => Middleware = middleware;
}

/// <summary> An <see cref="ICrawlerMiddlewareFactory"/> for handling the <see cref="DelegateCrawlerMiddlewareDescriptor"/>. </summary>
public class DelegateCrawlerMiddlewareFactory : CrawlerMiddlewareFactory<DelegateCrawlerMiddlewareDescriptor>
{
    /// <inheritdoc/>
    public override ICrawlerMiddleware Create( DelegateCrawlerMiddlewareDescriptor descriptor )
        => new DelegateCrawlerMiddleware( descriptor.Middleware );

    private class DelegateCrawlerMiddleware : ICrawlerMiddleware
    {
        #region Fields
        private readonly Func<CrawlUrlContext, CrawlUrlDelegate, Task> middleware;
        #endregion

        public DelegateCrawlerMiddleware( Func<CrawlUrlContext, CrawlUrlDelegate, Task> middleware )
            => this.middleware = middleware;

        /// <inheritdoc/>
        public Task InvokeAsync( CrawlUrlContext context, CrawlUrlDelegate next )
            => middleware( context, next );
    }
}