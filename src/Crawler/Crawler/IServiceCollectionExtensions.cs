using System;
using Earl.Crawler.Abstractions;
using Earl.Crawler.Infrastructure.Abstractions;
using Earl.Crawler.Infrastructure.Html;
using Earl.Crawler.Infrastructure.Http;
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

            services.AddHttpClient();
            services.TryAddSingleton( new RecyclableMemoryStreamManager() );

            services.AddTransient<IEarlCrawler, EarlCrawler>();
            services.AddTransient<ICrawlRequestMiddlewareInvoker, CrawlRequestMiddlewareInvoker>();
            services.AddScoped<ICrawlRequestMiddleware, HttpResponseMiddleware>();
            services.AddScoped<ICrawlRequestMiddleware, HtmlDocumentMiddleware>();

            services.AddScoped<ICrawlRequestMiddleware, UrlScraperMiddleware>();
            services.AddScoped<IUrlScraper, UrlScraper>();

            return services;
        }

    }

}
