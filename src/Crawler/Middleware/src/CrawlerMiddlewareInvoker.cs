using Earl.Crawler.Events;
using Earl.Crawler.Middleware.Abstractions;
using Earl.Crawler.Middleware.Events;

namespace Earl.Crawler.Middleware;

/// <summary> Default implementation of <see cref="ICrawlerMiddlewareInvoker"/>. </summary>
public sealed class CrawlerMiddlewareInvoker : ICrawlerMiddlewareInvoker
{
    private readonly ICrawlerMiddlewareFactory middlewareFactory;

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
                await middleware.InvokeAsync( context, next )
                    .ConfigureAwait( false );

                await context.OnProgressAsync()
                    .ConfigureAwait( false );
            };

    private Stack<ICrawlerMiddleware> CreateMiddlewareStack( CrawlUrlContext context )
    {
        var middlewares = context.CrawlContext.Options.Middleware
            .Select( middlewareFactory.Create );

        return new Stack<ICrawlerMiddleware>( middlewares );
    }
}