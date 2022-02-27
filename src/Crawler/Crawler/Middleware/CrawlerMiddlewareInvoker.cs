using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Middleware.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Middleware;

/// <summary> Default implementation of <see cref="ICrawlerMiddlewareInvoker"/>. </summary>
public class CrawlerMiddlewareInvoker : ICrawlerMiddlewareInvoker
{
    #region Fields
    private static readonly Type ICrawlerMiddlewareFactoryOfTType = typeof( ICrawlerMiddlewareFactory<> );
    #endregion

    /// <inheritdoc/>
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

    private static ICrawlerMiddleware CreateMiddleware( CrawlUrlContext context, ICrawlerMiddlewareDescriptor descriptor )
    {
        var factoryType = ICrawlerMiddlewareFactoryOfTType.MakeGenericType( descriptor.GetType() );
        var factory = ( ICrawlerMiddlewareFactory )context.Services.GetRequiredService( factoryType );
        return factory.Create( descriptor );
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
        var middlewares = context.CrawlContext.Options.Middleware
            .Select( descriptor => CreateMiddleware( context, descriptor ) )
            .Reverse();

        return middlewares.Any() is true
            ? new Stack<ICrawlerMiddleware>( middlewares ) : null;
    }
}