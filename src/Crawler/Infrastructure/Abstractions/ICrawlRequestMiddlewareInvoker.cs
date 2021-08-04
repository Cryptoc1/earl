using System.Threading.Tasks;

namespace Earl.Crawler.Infrastructure.Abstractions
{

    public interface ICrawlRequestMiddlewareInvoker
    {

        Task InvokeAsync( CrawlRequestContext context );

    }

}
