﻿using Earl.Crawler.Abstractions;
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
