namespace Earl.Crawler.Infrastructure.Abstractions
{

    /// <summary> A function that can process a url crawl. </summary>
    /// <param name="context"> The <see cref="CrawlUrlContext"/> for the url crawl. </param>
    /// <returns> A task that represents the completion of processing. </returns>
    public delegate Task CrawlUrlDelegate( CrawlUrlContext context );

}
