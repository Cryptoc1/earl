using Earl.Crawler.Infrastructure.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler
{

    public class CrawlerMiddlewareInvoker : ICrawlUrlMiddlewareInvoker
    {

        public async Task InvokeAsync( CrawlUrlContext context )
        {
            var middlewares = GetMiddlewareStack( context.Services );
            if( middlewares is null )
            {
                return;
            }

            var middleware = GetNextMiddleware( middlewares );
            await middleware( context );
        }

        private CrawlUrlDelegate GetNextMiddleware( Stack<ICrawlUrlMiddleware> middlewares )
            => !middlewares.TryPop( out var middleware )
                ? _ => Task.CompletedTask
                : async context =>
                {
                    var next = GetNextMiddleware( middlewares );
                    await middleware.InvokeAsync( context, next );
                };

        private static Stack<ICrawlUrlMiddleware>? GetMiddlewareStack( IServiceProvider services )
        {
            var middlewares = services.GetService<IEnumerable<ICrawlUrlMiddleware>>();
            if( middlewares?.Any() is not true )
            {
                return null;
            }

            // TODO: sort by annotations
            return new Stack<ICrawlUrlMiddleware>( middlewares.Reverse() );
        }

    }

}
