using Earl.Crawler.Abstractions;
using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Middleware;
using Earl.Crawler.Middleware.Abstractions;
using Earl.Crawler.Middleware.Http;
using Earl.Crawler.Middleware.UrlScraping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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

        AddCrawlerMiddlewareFactory<DelegateCrawlerMiddlewareDescriptor, DelegateCrawlerMiddlewareFactory>( services );
        AddCrawlerMiddlewareFactory<ServiceCrawlerMiddlewareDescriptor, ServiceCrawlerMiddlewareFactory>( services );

        // TODO: make this public, move the Earl.Crawler.Middleware
        static IServiceCollection AddCrawlerMiddlewareFactory<TDescriptor, TFactory>( IServiceCollection services )
            where TDescriptor : ICrawlerMiddlewareDescriptor
            where TFactory : CrawlerMiddlewareFactory<TDescriptor>
        {
            ArgumentNullException.ThrowIfNull( services );

            services.TryAddTransient<ICrawlerMiddlewareFactory, CrawlerMiddlewareFactory>();
            return services.AddTransient<CrawlerMiddlewareFactory<TDescriptor>, TFactory>();
        }
    }
}