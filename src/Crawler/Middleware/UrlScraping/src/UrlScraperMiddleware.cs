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
    private readonly IUrlFilterInvoker filterInvoker;
    private readonly UrlScraperOptions options;
    private readonly IUrlScraperInvoker scraperInvoker;

    public UrlScraperMiddleware(
        IUrlFilterInvoker filterInvoker,
        UrlScraperOptions options,
        IUrlScraperInvoker scraperInvoker
    )
    {
        this.filterInvoker = filterInvoker;
        this.options = options;
        this.scraperInvoker = scraperInvoker;
    }

    /// <inheritdoc/>
    public async Task InvokeAsync( CrawlUrlContext context, CrawlUrlDelegate next )
    {
        ArgumentNullException.ThrowIfNull( context );
        ArgumentNullException.ThrowIfNull( next );

        var feature = context.Features.Get<IHtmlDocumentFeature?>();
        if( feature is null )
        {
            await next( context );
            return;
        }

        var urls = scraperInvoker.InvokeAsync( options.Scrapers, feature.Document, context.CrawlContext.CrawlCancelled );

        // NOTE: don't immediately await
        var enqueue = filterInvoker.InvokeAsync( options.Filters, feature.Document, urls, context.CrawlContext.CrawlCancelled )
            .ForEachAsync(
                url => context.CrawlContext.UrlQueue.Enqueue( url ),
                context.CrawlContext.CrawlCancelled
            );

        await next( context );

        // wait for urls to be add to the queue
        await enqueue.ConfigureAwait( false );
    }
}