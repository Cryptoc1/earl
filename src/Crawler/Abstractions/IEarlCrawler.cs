using System;
using System.Threading;
using System.Threading.Tasks;

namespace Earl.Crawler.Abstractions
{

    public interface IEarlCrawler
    {

        Task<CrawlResult> CrawlAsync( Uri initiator, ICrawlOptions? options = null, CancellationToken cancellation = default );

    }

}
