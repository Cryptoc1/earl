using Earl.Crawler.Abstractions;
using Earl.Crawler.Middleware;
using Earl.Crawler.Middleware.Abstractions;
using Earl.Crawler.Middleware.Http;
using Earl.Crawler.Middleware.Http.Abstractions;
using Earl.Crawler.Middleware.UrlScraping;
using Earl.Crawler.Middleware.UrlScraping.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler;

/// <summary> Extensions to <see cref="IServiceCollection"/> for registering an <see cref="IEarlCrawler"/>. </summary>
public static class EarlServiceCollectionExtensions
{
    /// <summary> Adds service dependencies for <see cref="IEarlCrawler"/>. </summary>
    /// <param name="services"> The <see cref="IServiceCollection"/> to add dependencies to. </param>
    public static IServiceCollection AddEarlCrawler( this IServiceCollection services )
    {
        ArgumentNullException.ThrowIfNull( services );

        services.AddTransient<EarlHttpMessageHandler>();
        services.AddHttpClient<IEarlHttpClient, EarlHttpClient>()
            .AddHttpMessageHandler<EarlHttpMessageHandler>()
            .SetHandlerLifetime( TimeSpan.FromMinutes( 5 ) );

        services.AddTransient<IEarlCrawler, EarlCrawler>();

        services.AddTransient<ICrawlerMiddlewareFactory<DelegateCrawlerMiddlewareDescriptor>, DelegateCrawlerMiddlewareFactory>();
        services.AddTransient<ICrawlerMiddlewareFactory<ServiceCrawlerMiddlewareDescriptor>, ServiceCrawlerMiddlewareFactory>();
        services.AddTransient<ICrawlerMiddlewareInvoker, CrawlerMiddlewareInvoker>();

        services.AddScoped<IUrlScraper, UrlScraper>();

        return services;
    }
}