using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Middleware.Abstractions;

namespace Earl.Crawler.Middleware.Configuration;

/// <summary> Extensions to <see cref="ICrawlerOptionsBuilder"/> for registering <see cref="ICrawlerMiddlewareDescriptor"/>s. </summary>
public static class MiddlewareCrawlerOptionsBuilderExtensions
{
    /// <summary> Register the middleware of type <typeparamref name="TMiddleware"/>. </summary>
    /// <typeparam name="TMiddleware"> The type of middleware to be registered. </typeparam>
    /// <param name="builder"> The options builder to register the handler with. </param>
    public static ICrawlerOptionsBuilder Use<TMiddleware>( this ICrawlerOptionsBuilder builder )
        where TMiddleware : ICrawlerMiddleware
        => Use( builder, new ServiceCrawlerMiddlewareDescriptor( typeof( TMiddleware ) ) );

    /// <summary> Register the middleware of type <typeparamref name="TMiddleware"/> with the given <paramref name="options"/>. </summary>
    /// <typeparam name="TMiddleware"> The type of middleware to be registered. </typeparam>
    /// <typeparam name="TOptions"> The type of options to be registered. </typeparam>
    /// <param name="builder"> The options builder to register the handler with. </param>
    /// <param name="options"> The middleware options to register. </param>
    public static ICrawlerOptionsBuilder Use<TMiddleware, TOptions>( this ICrawlerOptionsBuilder builder, TOptions options )
        where TMiddleware : ICrawlerMiddleware<TOptions>
        => Use( builder, new ServiceCrawlerMiddlewareDescriptor( typeof( TMiddleware ), options ) );

    /// <summary> Register the given delegate as middleware. </summary>
    /// <param name="builder"> The options builder to register the handler with. </param>
    /// <param name="middleware"> The delegate to register as middleware. </param>
    public static ICrawlerOptionsBuilder Use( this ICrawlerOptionsBuilder builder, Func<CrawlUrlContext, CrawlUrlDelegate, Task> middleware )
    {
        ArgumentNullException.ThrowIfNull( middleware );
        return Use( builder, new DelegateCrawlerMiddlewareDescriptor( middleware ) );
    }

    /// <summary> Register the middleware descriptor. </summary>
    /// <param name="builder"> The options builder to register the handler with. </param>
    /// <param name="descriptor"> The <see cref="ICrawlerMiddlewareDescriptor"/> to register. </param>
    public static ICrawlerOptionsBuilder Use( this ICrawlerOptionsBuilder builder, ICrawlerMiddlewareDescriptor descriptor )
    {
        ArgumentNullException.ThrowIfNull( builder );
        ArgumentNullException.ThrowIfNull( descriptor );

        return builder.Configure(
            ( _, options ) => options with
            {
                Middleware = options.Middleware
                    .Prepend( descriptor )
                    .ToList(),
            }
        );
    }
}