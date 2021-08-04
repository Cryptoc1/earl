using System.Threading.Tasks;

namespace Earl.Crawler.Infrastructure.Abstractions
{

    public delegate Task CrawlRequestDelegate( CrawlRequestContext context );

}
