using Earl.Crawler.Abstractions;
using Earl.Crawler.Infrastructure.Abstractions;
using Earl.Crawler.Infrastructure.Html;
using Earl.Crawler.Infrastructure.Http;
using Earl.Crawler.Infrastructure.Http.Abstractions;
using Earl.Crawler.Infrastructure.Selenium;
using Earl.Crawler.Infrastructure.UrlScraper;
using Earl.Crawler.Infrastructure.UrlScraper.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IO;

namespace Earl.Crawler
{

    public static class IServiceCollectionExtensions
    {

        public static IServiceCollection AddEarlCrawler( this IServiceCollection services )
        {
            if( services is null )
            {
                throw new ArgumentNullException( nameof( services ) );
            }

            services.TryAddSingleton( _ => new RecyclableMemoryStreamManager() );

            services.AddTransient<EarlHttpMessageHandler>();
            services.AddHttpClient<IEarlHttpClient, EarlHttpClient>()
                .AddHttpMessageHandler<EarlHttpMessageHandler>()
                .SetHandlerLifetime( TimeSpan.FromMinutes( 5 ) );

            services.AddTransient<IEarlCrawler, EarlCrawler>();
            services.AddTransient<ICrawlUrlMiddlewareInvoker, CrawlerMiddlewareInvoker>();

            services.AddScoped<ICrawlUrlMiddleware, HttpResponseMiddleware>();
            services.AddScoped<ICrawlUrlMiddleware, HtmlDocumentMiddleware>();
            services.AddScoped<ICrawlUrlMiddleware, SeleniumMiddleware>();

            services.AddScoped<ICrawlUrlMiddleware, UrlScraperMiddleware>();
            services.AddScoped<IUrlScraper, UrlScraper>();

            return services;
        }

    }

}
