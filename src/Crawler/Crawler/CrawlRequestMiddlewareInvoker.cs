using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Earl.Crawler.Infrastructure.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Earl.Crawler
{

    public class CrawlRequestMiddlewareInvoker : ICrawlRequestMiddlewareInvoker
    {
        #region Fields
        private readonly ILogger logger;
        #endregion

        public CrawlRequestMiddlewareInvoker( ILogger<CrawlRequestMiddlewareInvoker> logger )
            => this.logger = logger;

        public async Task InvokeAsync( CrawlRequestContext context )
        {
            var middlewares = GetMiddlewares( context.Services );
            if( middlewares?.Any() is not true )
            {
                return;
            }

            var middleware = middlewares.Pop();
            await middleware.InvokeAsync(
                context,
                CreateCrawlRequestDelegate( middlewares )
            );
        }

        private CrawlRequestDelegate CreateCrawlRequestDelegate( Stack<ICrawlRequestMiddleware> middlewares )
        {
            if( middlewares.TryPop( out var middleware ) )
            {
                return context =>
                {
                    logger.LogDebug( $"Invoking Middleware: '{middleware.GetType().Name}'." );
                    return middleware.InvokeAsync(
                        context,
                        CreateCrawlRequestDelegate( middlewares )
                    );
                };
            }

            return _ => Task.CompletedTask;
        }

        private Stack<ICrawlRequestMiddleware>? GetMiddlewares( IServiceProvider services )
        {
            var middlewares = services.GetService<IEnumerable<ICrawlRequestMiddleware>>()
                ?.ToList();

            if( middlewares?.Any() is not true )
            {
                return null;
            }

            // TODO: sort by annotations
            middlewares.Reverse();

            return new Stack<ICrawlRequestMiddleware>( middlewares );
        }

    }

}
