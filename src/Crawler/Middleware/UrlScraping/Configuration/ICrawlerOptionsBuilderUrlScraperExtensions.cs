using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Middleware.UrlScraping.Abstractions.Configuration;
using Earl.Crawler.Middleware.UrlScraping.Filters;

namespace Earl.Crawler.Middleware.UrlScraping.Configuration;

/// <summary> Extensions to <see cref="ICrawlerOptionsBuilder"/> for configuring the <see cref="UrlScraperMiddleware"/>. </summary>
public static class ICrawlerOptionsBuilderUrlScraperExtensions
{
    private static readonly Type UrlScraperMiddlewareType = typeof( UrlScraperMiddleware );

    private static UrlScraperOptions CreateDefaultScraperOptions( )
        => new UrlScraperOptions( new List<IUrlFilterDescriptor>() )
            .WithFilter<SameOriginUrlFilter>();

    /// <summary> Register the <see cref="UrlScraperMiddleware"/>. </summary>
    /// <param name="builder"> The builder to register the middleware to. </param>
    /// <param name="configure"> A delegate method that configures the <see cref="UrlScraperOptions"/>. </param>
    public static ICrawlerOptionsBuilder UseUrlScraper( this ICrawlerOptionsBuilder builder, Func<UrlScraperOptions, UrlScraperOptions>? configure = null )
    {
        ArgumentNullException.ThrowIfNull( builder );
        return builder.Configure(
            ( _, options ) =>
            {
                var middleware = options.Middleware.ToList();
                var existingDescriptor = middleware.OfType<ServiceCrawlerMiddlewareDescriptor>()
                    .FirstOrDefault( descriptor => descriptor.MiddlewareType == UrlScraperMiddlewareType );

                var middlewareOptions = ( existingDescriptor?.Options as UrlScraperOptions )
                    ?? CreateDefaultScraperOptions();

                if( configure is not null )
                {
                    middlewareOptions = configure( middlewareOptions );
                }

                var descriptor = new ServiceCrawlerMiddlewareDescriptor( UrlScraperMiddlewareType, middlewareOptions );
                int insertAt = 0;

                if( existingDescriptor is not null )
                {
                    insertAt = middleware.IndexOf( existingDescriptor );
                    middleware.RemoveAt( insertAt );
                }

                middleware.Insert( insertAt, descriptor );
                return options with
                {
                    Middleware = middleware,
                };
            }
        );
    }
}