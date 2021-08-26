namespace Earl.Crawler.Abstractions
{

    public interface ICrawlReporter
    {

        Task OnUrlCrawlComplete( CrawlUrlResult result );

    }

}
