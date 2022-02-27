using Earl.Crawler.Middleware.UrlScraping.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Middleware.UrlScraping;

/// <summary> Extensions to <see cref="IServiceCollection"/>. </summary>
public static class UrlScrapingServiceCollectionExtensions
{
    /// <summary> Adds dependencies for the <see cref="UrlScraperMiddleware"/>. </summary>
    /// <param name="services"> The <see cref="IServiceCollection"/> to add dependencies to. </param>
    public static IServiceCollection AddEarlUrlScraping( this IServiceCollection services )
    {
        services.AddScoped<IUrlScraper, UrlScraper>();

        return services;
    }
}