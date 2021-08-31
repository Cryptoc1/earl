namespace Earl.Crawler.Abstractions
{

    public interface ICrawlHandler
    {

        Task OnCrawlUrlResult( CrawlUrlResult result, CancellationToken cancellation = default );

    }

}
