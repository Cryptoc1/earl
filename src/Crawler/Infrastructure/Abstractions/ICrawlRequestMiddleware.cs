using System.Threading.Tasks;

namespace Earl.Crawler.Infrastructure.Abstractions
{

    public interface ICrawlRequestMiddleware
    {

        Task InvokeAsync( CrawlRequestContext context, CrawlRequestDelegate next );

    }

}
