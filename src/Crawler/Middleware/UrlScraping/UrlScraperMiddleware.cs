using Earl.Crawler.Middleware.Abstractions;
using Earl.Crawler.Middleware.Html;
using Earl.Crawler.Middleware.UrlScraping.Abstractions;
using Earl.Crawler.Middleware.UrlScraping.Abstractions.Configuration;

namespace Earl.Crawler.Middleware.UrlScraping;

/// <summary> Scrapes and queues urls for the current url crawl. </summary>
/// <remarks> Requires that the <see cref="HtmlDocumentMiddleware"/>, or compatible middleware is registered before this middleware in the pipeline. </remarks>
public class UrlScraperMiddleware : ICrawlerMiddleware<UrlScraperOptions>
{
    #region Fields
    private readonly UrlScraperOptions options;
    private readonly IUrlScraper scraper;
    #endregion

    public UrlScraperMiddleware( UrlScraperOptions options, IUrlScraper scraper )
    {
        this.options = options;
        this.scraper = scraper;
    }

    /// <inheritdoc/>
    public async Task InvokeAsync( CrawlUrlContext context, CrawlUrlDelegate next )
    {
        ArgumentNullException.ThrowIfNull( context );
        ArgumentNullException.ThrowIfNull( next );

        var documentFeature = context.Features.Get<IHtmlDocumentFeature?>();
        if( documentFeature is null )
        {
            return;
        }

        var urls = scraper.ScrapeAsync( documentFeature.Document, options, context.CrawlContext.CrawlCancelled );

        // NOTE: don't immediately await, invoke the rest of the middleware pipeline first
        var enqueue = urls.ForEachAsync(
            url => context.CrawlContext.UrlQueue.Enqueue( url ),
            context.CrawlContext.CrawlCancelled
        );

        await next( context );

        // wait for urls to queue
        await enqueue;
    }
}