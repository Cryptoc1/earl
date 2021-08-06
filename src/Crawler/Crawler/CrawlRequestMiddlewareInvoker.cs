using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Earl.Crawler.Infrastructure.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler
{

    public class CrawlRequestMiddlewareInvoker : ICrawlRequestMiddlewareInvoker
    {

        public async Task InvokeAsync( CrawlRequestContext context )
        {
            var middlewares = GetMiddlewareStack( context.Services );
            if( middlewares is null )
            {
                return;
            }

            var middleware = GetNextMiddleware( middlewares );
            await middleware( context );
        }

        private CrawlRequestDelegate GetNextMiddleware( Stack<ICrawlRequestMiddleware> middlewares )
            => !middlewares.TryPop( out var middleware )
                ? _ => Task.CompletedTask
                : async context =>
                {
                    var next = GetNextMiddleware( middlewares );
                    await middleware.InvokeAsync( context, next );
                };

        private static Stack<ICrawlRequestMiddleware>? GetMiddlewareStack( IServiceProvider services )
        {
            var middlewares = services.GetService<IEnumerable<ICrawlRequestMiddleware>>();
            if( middlewares?.Any() is not true )
            {
                return null;
            }

            // TODO: sort by annotations
            return new Stack<ICrawlRequestMiddleware>( middlewares.Reverse() );
        }

    }

}
