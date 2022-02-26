using Earl.Crawler.Abstractions;
using Earl.Crawler.Middleware.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Middleware;

/// <summary> Default implementation of <see cref="ICrawlerMiddlewareInvoker"/>. </summary>
public class CrawlerMiddlewareInvoker : ICrawlerMiddlewareInvoker
{
    public async Task InvokeAsync( CrawlUrlContext context )
    {
        ArgumentNullException.ThrowIfNull( context );

        var middlewares = CreateMiddlewareStack( context );
        if( middlewares is null )
        {
            return;
        }

        var pipeline = CreateMiddlewarePipeline( middlewares );
        await pipeline( context );
    }

    private static ICrawlerMiddleware CreateMiddleware( CrawlUrlContext context, ICrawlerMiddlewareDefinition defintion )
    {
        var factoryType = typeof( ICrawlerMiddlewareFactory<> )
            .MakeGenericType( defintion.GetType() );

        var factory = ( ICrawlerMiddlewareFactory )context.Services.GetRequiredService( factoryType );
        return factory.Create( defintion );
    }

    private CrawlUrlDelegate CreateMiddlewarePipeline( Stack<ICrawlerMiddleware> middlewares )
        => !middlewares.TryPop( out var middleware )
            ? _ => Task.CompletedTask
            : async context =>
            {
                context.CrawlContext.CrawlCancelled.ThrowIfCancellationRequested();

                var next = CreateMiddlewarePipeline( middlewares );
                await middleware.InvokeAsync( context, next );
            };

    private static Stack<ICrawlerMiddleware>? CreateMiddlewareStack( CrawlUrlContext context )
    {
        var middlewares = context.CrawlContext.Options.Middleware.Select(
            definition => CreateMiddleware( context, definition )
        );

        if( middlewares?.Any() is not true )
        {
            return null;
        }

        // TODO: sort by annotations
        return new Stack<ICrawlerMiddleware>( middlewares.Reverse() );
    }
}