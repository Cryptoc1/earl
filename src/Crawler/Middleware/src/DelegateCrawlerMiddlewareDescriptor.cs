using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Middleware.Abstractions;

namespace Earl.Crawler.Middleware;

/// <summary> Represents an <see cref="ICrawlerMiddlewareDescriptor"/> for a delegate method. </summary>
public sealed class DelegateCrawlerMiddlewareDescriptor : ICrawlerMiddlewareDescriptor
{
    /// <summary> The delegate method representing the <see cref="ICrawlerMiddleware.InvokeAsync(CrawlUrlContext, CrawlUrlDelegate)"/> body. </summary>
    public Func<CrawlUrlContext, CrawlUrlDelegate, Task> Middleware { get; }

    public DelegateCrawlerMiddlewareDescriptor( Func<CrawlUrlContext, CrawlUrlDelegate, Task> middleware )
    {
        ArgumentNullException.ThrowIfNull( middleware );
        Middleware = middleware;
    }
}

/// <summary> An <see cref="ICrawlerMiddlewareFactory"/> for handling the <see cref="DelegateCrawlerMiddlewareDescriptor"/>. </summary>
public class DelegateCrawlerMiddlewareFactory : CrawlerMiddlewareFactory<DelegateCrawlerMiddlewareDescriptor>
{
    /// <inheritdoc/>
    public override ICrawlerMiddleware Create( DelegateCrawlerMiddlewareDescriptor descriptor )
        => new DelegateCrawlerMiddleware( descriptor.Middleware );

    private sealed class DelegateCrawlerMiddleware : ICrawlerMiddleware<Func<CrawlUrlContext, CrawlUrlDelegate, Task>>
    {
        private readonly Func<CrawlUrlContext, CrawlUrlDelegate, Task> middleware;

        public DelegateCrawlerMiddleware( Func<CrawlUrlContext, CrawlUrlDelegate, Task> middleware )
            => this.middleware = middleware;

        /// <inheritdoc/>
        public Task InvokeAsync( CrawlUrlContext context, CrawlUrlDelegate next )
            => middleware( context, next );
    }
}