using Earl.Crawler.Middleware.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler;

public class CrawlerMiddlewareInvoker : ICrawlerMiddlewareInvoker
{
    public async Task InvokeAsync( CrawlUrlContext context )
    {
        if( context is null )
        {
            throw new ArgumentNullException( nameof( context ) );
        }

        var middlewares = CreateMiddlewareStack( context.Services );
        if( middlewares is null )
        {
            return;
        }

        var pipeline = CreateMiddlewarePipeline( middlewares );
        await pipeline( context );
    }

    private CrawlUrlDelegate CreateMiddlewarePipeline( Stack<ICrawlerMiddleware> middlewares )
        => !middlewares.TryPop( out var middleware )
            ? _ => Task.CompletedTask
            : async context =>
            {
                var next = CreateMiddlewarePipeline( middlewares );
                await middleware.InvokeAsync( context, next );
            };

    private static Stack<ICrawlerMiddleware>? CreateMiddlewareStack( IServiceProvider services )
    {
        var middlewares = services.GetService<IEnumerable<ICrawlerMiddleware>>();
        if( middlewares?.Any() is not true )
        {
            return null;
        }

        // TODO: sort by annotations
        return new Stack<ICrawlerMiddleware>( middlewares.Reverse() );
    }
}