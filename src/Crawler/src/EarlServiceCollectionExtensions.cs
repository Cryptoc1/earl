using Earl.Crawler.Abstractions;
using Earl.Crawler.Middleware;
using Earl.Crawler.Middleware.Abstractions;
using Earl.Crawler.Middleware.Http;
using Earl.Crawler.Middleware.UrlScraping;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler;

/// <summary> Extensions to <see cref="IServiceCollection"/>. </summary>
public static class EarlServiceCollectionExtensions
{
    /// <summary> Adds service dependencies for <see cref="IEarlCrawler"/>. </summary>
    /// <param name="services"> The <see cref="IServiceCollection"/> to add dependencies to. </param>
    public static IServiceCollection AddEarlCrawler( this IServiceCollection services )
    {
        ArgumentNullException.ThrowIfNull( services );

        services.AddTransient<IEarlCrawler, EarlCrawler>();

        AddMiddlewareServices( services );
        services.AddEarlHttpResponse();
        services.AddEarlUrlScraper();

        return services;
    }

    private static void AddMiddlewareServices( IServiceCollection services )
    {
        services.AddTransient<ICrawlerMiddlewareInvoker, CrawlerMiddlewareInvoker>();

        services.AddTransient<ICrawlerMiddlewareFactory, CrawlerMiddlewareFactory>();
        services.AddTransient<CrawlerMiddlewareFactory<DelegateCrawlerMiddlewareDescriptor>, DelegateCrawlerMiddlewareFactory>();
        services.AddTransient<CrawlerMiddlewareFactory<ServiceCrawlerMiddlewareDescriptor>, ServiceCrawlerMiddlewareFactory>();
    }
}