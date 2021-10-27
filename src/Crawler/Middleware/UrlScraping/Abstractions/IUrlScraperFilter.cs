namespace Earl.Crawler.Middleware.UrlScraping.Abstractions
{

    /// <summary> Describes a service that can filters urls scraped via <see cref="IUrlScraper.ScrapeAsync(AngleSharp.Html.Dom.IHtmlDocument, Uri, CancellationToken)"/>. </summary>
    public interface IUrlScraperFilter
    {

        /// <summary> Filters the given <paramref name="urls"/>. </summary>
        /// <param name="urls"> The urls to filter. </param>
        /// <returns> The filtered urls. </returns>
        Task<IEnumerable<Uri>> FilterAsync( IEnumerable<Uri> urls, CancellationToken cancellation = default );

    }

}
