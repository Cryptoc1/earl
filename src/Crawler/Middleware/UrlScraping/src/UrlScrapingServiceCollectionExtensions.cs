using Earl.Crawler.Middleware.UrlScraping.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Middleware.UrlScraping;

/// <summary> Extensions to <see cref="IServiceCollection"/>. </summary>
public static class UrlScrapingServiceCollectionExtensions
{
    /// <summary> Adds dependencies for the <see cref="UrlScraperMiddleware"/>. </summary>
    /// <param name="services"> The <see cref="IServiceCollection"/> to add dependencies to. </param>
    public static IServiceCollection AddEarlUrlScraper( this IServiceCollection services )
    {
        services.AddTransient<IUrlScraperInvoker, UrlScraperInvoker>();
        services.AddTransient<IUrlScraperFactory, UrlScraperFactory>();
        services.AddTransient<UrlScraperFactory<ServiceUrlScraperDescriptor>, ServiceUrlScraperFactory>();

        services.AddTransient<IUrlFilterInvoker, UrlFilterInvoker>();
        services.AddTransient<IUrlFilterFactory, UrlFilterFactory>();
        services.AddTransient<UrlFilterFactory<ServiceUrlFilterDescriptor>, ServiceUrlFilterFactory>();

        return services;
    }
}