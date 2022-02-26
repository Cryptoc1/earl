using Earl.Crawler.Middleware.Abstractions;
using Earl.Crawler.Middleware.Html;
using Earl.Crawler.Middleware.UrlScraping.Abstractions;

namespace Earl.Crawler.Middleware.UrlScraping;

/// <summary> Scrapes and queues urls for the current url crawl to be crawled. </summary>
/// <remarks> Requires that the <see cref="HtmlDocumentMiddleware"/>, or compatible middleware is registered in before this middleware in the pipeline. </remarks>
public class UrlScraperMiddleware : ICrawlerMiddleware
{
    #region Fields
    private readonly IUrlScraper scraper;
    #endregion

    public UrlScraperMiddleware( IUrlScraper scraper )
        => this.scraper = scraper;

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

        var urls = scraper.ScrapeAsync(
            documentFeature.Document,
            new Uri( context.CrawlContext.Initiator.GetLeftPart( UriPartial.Authority ) ),
            context.CrawlContext.CrawlCancelled
        );

        // NOTE: don't initially await the enqueue task, we can keep processing the middleware pipeline
        var enqueue = urls.ForEachAsync(
            url => context.CrawlContext.UrlQueue.Enqueue( url ),
            context.CrawlContext.CrawlCancelled
        );

        await next( context );

        // ensure enqueue task is awaited to prevent uncancellable work
        await enqueue;
    }
}