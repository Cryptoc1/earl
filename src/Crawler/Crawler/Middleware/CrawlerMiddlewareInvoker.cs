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

                var progress = context.CrawlContext.EmitProgressAsync();
                await middleware.InvokeAsync(
                    context,
                    PopMiddlewareDelegate( middlewares )
                );

                await progress;
            };

    private Stack<ICrawlerMiddleware> CreateMiddlewareStack( CrawlUrlContext context )
    {
        var middlewares = context.CrawlContext.Options.Middleware
            .Reverse()
            .Select( descriptor => middlewareFactory.Create( descriptor ) );

        return new Stack<ICrawlerMiddleware>( middlewares );
    }
}