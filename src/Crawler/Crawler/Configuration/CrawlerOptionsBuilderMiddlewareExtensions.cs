using Earl.Crawler.Abstractions;
using Earl.Crawler.Middleware;
using Earl.Crawler.Middleware.Abstractions;

namespace Earl.Crawler.Configuration;

/// <summary> Extensions to <see cref="ICrawlerOptionsBuilder"/> for registering <see cref="ICrawlerMiddlewareDefinition"/>s. </summary>
public static class CrawlerOptionsBuilderMiddlewareExtensions
{
    #region Fields
    private static readonly string MiddlewareKey = nameof( MiddlewareKey );
    #endregion

    private static void BuildMiddleware( ICrawlerOptionsBuilder builder, ICrawlerOptions options )
    {
        ArgumentNullException.ThrowIfNull( builder );
        ArgumentNullException.ThrowIfNull( options );

        foreach( var middleware in MiddlewareProperty( builder ) )
        {
            options.Middleware.Add( middleware );
        }
    }

    private static IList<ICrawlerMiddlewareDefinition> MiddlewareProperty( ICrawlerOptionsBuilder builder )
        => builder.GetOrAddProperty( MiddlewareKey, ( ) => new List<ICrawlerMiddlewareDefinition>() );

    /// <summary> Register the middleware of type <typeparamref name="TMiddleware"/>. </summary>
    /// <typeparam name="TMiddleware"> The type of middleware to be registered. </typeparam>
    /// <param name="builder"> The options builder to register the handler with. </param>
    public static ICrawlerOptionsBuilder UseMiddleware<TMiddleware>( this ICrawlerOptionsBuilder builder )
        where TMiddleware : ICrawlerMiddleware
    {
        ArgumentNullException.ThrowIfNull( builder );

        MiddlewareProperty( builder )
            .Add( new ServiceCrawlerMiddlewareDefinition( typeof( TMiddleware ) ) );

        return CrawlerOptionsBuilder.Decorate( builder, BuildMiddleware );
    }

    /// <summary> Register the given delegate as middleware. </summary>
    /// <param name="builder"> The options builder to register the handler with. </param>
    /// <param name="middleware"> The delegate to register as middleware. </param>
    public static ICrawlerOptionsBuilder UseMiddleware( this ICrawlerOptionsBuilder builder, Func<CrawlUrlContext, CrawlUrlDelegate, Task> middleware )
    {
        ArgumentNullException.ThrowIfNull( builder );

        MiddlewareProperty( builder )
            .Add( new DelegateCrawlerMiddlewareDefinition( middleware ) );

        return CrawlerOptionsBuilder.Decorate( builder, BuildMiddleware );
    }
}