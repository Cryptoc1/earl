namespace Earl.Crawler.Abstractions
{

    public interface ICrawlHandler
    {

        Task OnCrawledUrl( CrawlUrlResult result, CancellationToken cancellation = default );

    }

}
