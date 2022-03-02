using Earl.Crawler.Events;
using Earl.Crawler.Middleware.Abstractions;

namespace Earl.Crawler.Middleware;

/// <summary> Default implementation of <see cref="ICrawlerMiddlewareInvoker"/>. </summary>
public class CrawlerMiddlewareInvoker : ICrawlerMiddlewareInvoker
{
    #region Fields
    private readonly ICrawlerMiddlewareFactory middlewareFactory;
    #endregion

    public CrawlerMiddlewareInvoker( ICrawlerMiddlewareFactory middlewareFactory )
        => this.middlewareFactory = middlewareFactory;

    /// <inheritdoc/>
    public Task InvokeAsync( CrawlUrlContext context )
    {
        ArgumentNullException.ThrowIfNull( context );

        var middlewares = CreateMiddlewareStack( context );
        return PopMiddlewareDelegate( middlewares )( context );
    }

    private CrawlUrlDelegate PopMiddlewareDelegate( Stack<ICrawlerMiddleware> middlewares )
        => !middlewares.TryPop( out var middleware )
            ? _ => Task.CompletedTask
            : async context =>
            {
                context.CrawlContext.CrawlCancelled.ThrowIfCancellationRequested();

                var next = PopMiddlewareDelegate( middlewares );
                await middleware.InvokeAsync( context, next );

                await context.OnProgressAsync();
            };

    private Stack<ICrawlerMiddleware> CreateMiddlewareStack( CrawlUrlContext context )
    {
        var middlewares = context.CrawlContext.Options.Middleware
            .Select( middlewareFactory.Create );

        return new Stack<ICrawlerMiddleware>( middlewares );
    }
}