using Earl.Crawler.Abstractions;
using Earl.Crawler.Middleware;
using Earl.Crawler.Middleware.Abstractions;

namespace Earl.Crawler.Configuration;

/// <summary> Extensions to <see cref="ICrawlerOptionsBuilder"/> for registering <see cref="ICrawlerMiddlewareDescriptor"/>s. </summary>
public static class CrawlerOptionsBuilderMiddlewareExtensions
{
    #region Fields
    private static readonly string MiddlewareDescriptorsKey = nameof( MiddlewareDescriptorsKey );
    #endregion

    private static void BuildMiddleware( ICrawlerOptionsBuilder builder, ICrawlerOptions options )
    {
        ArgumentNullException.ThrowIfNull( builder );
        ArgumentNullException.ThrowIfNull( options );

        foreach( var descriptor in MiddlewareDescriptorsProperty( builder ) )
        {
            options.Middleware.Add( descriptor );
        }
    }

    private static IList<ICrawlerMiddlewareDescriptor> MiddlewareDescriptorsProperty( ICrawlerOptionsBuilder builder )
        => builder.GetOrAddProperty( MiddlewareDescriptorsKey, ( ) => new List<ICrawlerMiddlewareDescriptor>() );

    /// <summary> Register the middleware of type <typeparamref name="TMiddleware"/>. </summary>
    /// <typeparam name="TMiddleware"> The type of middleware to be registered. </typeparam>
    /// <param name="builder"> The options builder to register the handler with. </param>
    public static ICrawlerOptionsBuilder UseMiddleware<TMiddleware>( this ICrawlerOptionsBuilder builder )
        where TMiddleware : ICrawlerMiddleware
    {
        ArgumentNullException.ThrowIfNull( builder );

        MiddlewareDescriptorsProperty( builder )
            .Add( new ServiceCrawlerMiddlewareDescriptor( typeof( TMiddleware ) ) );

        return CrawlerOptionsBuilder.Decorate( builder, BuildMiddleware );
    }

    /// <summary> Register the given delegate as middleware. </summary>
    /// <param name="builder"> The options builder to register the handler with. </param>
    /// <param name="middleware"> The delegate to register as middleware. </param>
    public static ICrawlerOptionsBuilder UseMiddleware( this ICrawlerOptionsBuilder builder, Func<CrawlUrlContext, CrawlUrlDelegate, Task> middleware )
    {
        ArgumentNullException.ThrowIfNull( builder );

        MiddlewareDescriptorsProperty( builder )
            .Add( new DelegateCrawlerMiddlewareDescriptor( middleware ) );

        return CrawlerOptionsBuilder.Decorate( builder, BuildMiddleware );
    }
}