using Earl.Crawler.Abstractions;
using Earl.Crawler.Middleware.Abstractions;
using Earl.Crawler.Middleware.Html;
using Earl.Crawler.Middleware.Http;
using Earl.Crawler.Middleware.Http.Abstractions;
using Earl.Crawler.Middleware.Selenium;
using Earl.Crawler.Middleware.UrlScraping;
using Earl.Crawler.Middleware.UrlScraping.Abstractions;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddTransient<EarlHttpMessageHandler>();
            services.AddHttpClient<IEarlHttpClient, EarlHttpClient>()
                .AddHttpMessageHandler<EarlHttpMessageHandler>()
                .SetHandlerLifetime( TimeSpan.FromMinutes( 5 ) );

            services.AddTransient<IEarlCrawler, EarlCrawler>();
            services.AddTransient<ICrawlerMiddlewareInvoker, CrawlerMiddlewareInvoker>();

            services.AddScoped<ICrawlerMiddleware, HttpResponseMiddleware>();
            services.AddScoped<ICrawlerMiddleware, HtmlDocumentMiddleware>();
            services.AddScoped<ICrawlerMiddleware, SeleniumMiddleware>();

            services.AddScoped<ICrawlerMiddleware, UrlScraperMiddleware>();
            services.AddScoped<IUrlScraper, UrlScraper>();

            return services;
        }

    }

}
