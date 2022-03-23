using Earl.Crawler.Middleware.Abstractions;
using Earl.Crawler.Middleware.Html;
using Earl.Crawler.Middleware.Html.Abstractions;
using Earl.Crawler.Middleware.UrlScraping.Abstractions;
using Earl.Crawler.Middleware.UrlScraping.Abstractions.Configuration;

namespace Earl.Crawler.Middleware.UrlScraping;

/// <summary> Scrapes and queues urls for the current url crawl. </summary>
/// <remarks> Requires that the <see cref="HtmlDocumentMiddleware"/>, or compatible middleware is registered before this middleware in the pipeline. </remarks>
public sealed class UrlScraperMiddleware : ICrawlerMiddleware<UrlScraperOptions>
{
    private readonly UrlScraperOptions options;
    private readonly IUrlScraper scraper;

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
            await next( context );
            return;
        }

        var urls = scraper.ScrapeAsync( documentFeature.Document, options, context.CrawlContext.CrawlCancelled );

        // NOTE: don't immediately await, invoke the remainder of the middleware pipeline first
        var enqueue = urls.ForEachAsync(
            url => context.CrawlContext.UrlQueue.Enqueue( url ),
            context.CrawlContext.CrawlCancelled
        ).ConfigureAwait( false );

        await next( context ).ConfigureAwait( false );

        // wait for urls to be add to the queue
        await enqueue;
    }
}