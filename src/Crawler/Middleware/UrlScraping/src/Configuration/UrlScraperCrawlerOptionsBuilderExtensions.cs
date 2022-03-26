using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Middleware.UrlScraping.Abstractions.Configuration;
using Earl.Crawler.Middleware.UrlScraping.Filters;
using Earl.Crawler.Middleware.UrlScraping.Scrapers;

namespace Earl.Crawler.Middleware.UrlScraping.Configuration;

/// <summary> Extensions to <see cref="ICrawlerOptionsBuilder"/> for configuring the <see cref="UrlScraperMiddleware"/>. </summary>
public static class UrlScraperCrawlerOptionsBuilderExtensions
{
    private static readonly Type UrlScraperMiddlewareType = typeof( UrlScraperMiddleware );

    private static UrlScraperOptions CreateDefaultScraperOptions( )
        => new UrlScraperOptions( new List<IUrlFilterDescriptor>(), new List<IUrlScraperDescriptor>() )
            .WithFilter<DistinctUrlFilter>()
            .WithFilter<SameOriginUrlFilter>()
            .WithScraper<AnchorUrlScraper>();

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
                var existingDescriptor = middleware.Find(
                    descriptor => descriptor is ServiceCrawlerMiddlewareDescriptor typedDescriptor && typedDescriptor.MiddlewareType == UrlScraperMiddlewareType
                ) as ServiceCrawlerMiddlewareDescriptor;

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