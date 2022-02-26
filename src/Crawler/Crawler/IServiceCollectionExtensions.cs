using Earl.Crawler.Abstractions;
using Earl.Crawler.Middleware;
using Earl.Crawler.Middleware.Abstractions;
using Earl.Crawler.Middleware.Http;
using Earl.Crawler.Middleware.Http.Abstractions;
using Earl.Crawler.Middleware.UrlScraping;
using Earl.Crawler.Middleware.UrlScraping.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddEarlCrawler( this IServiceCollection services )
    {
        ArgumentNullException.ThrowIfNull( services );

        services.AddTransient<EarlHttpMessageHandler>();
        services.AddHttpClient<IEarlHttpClient, EarlHttpClient>()
            .AddHttpMessageHandler<EarlHttpMessageHandler>()
            .SetHandlerLifetime( TimeSpan.FromMinutes( 5 ) );

        services.AddTransient<IEarlCrawler, EarlCrawler>();

        services.AddTransient<ICrawlerMiddlewareFactory<DelegateCrawlerMiddlewareDefinition>, DelegateCrawlerMiddlewareFactory>();
        services.AddTransient<ICrawlerMiddlewareFactory<ServiceCrawlerMiddlewareDefinition>, ServiceCrawlerMiddlewareFactory>();
        services.AddTransient<ICrawlerMiddlewareInvoker, CrawlerMiddlewareInvoker>();

        services.AddScoped<IUrlScraper, UrlScraper>();

        return services;
    }
}