﻿using Earl.Crawler.Middleware.Abstractions;
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

        var urls = await scraper.ScrapeAsync(
            documentFeature.Document,
            new Uri( context.CrawlContext.Initiator.GetLeftPart( UriPartial.Authority ) ),
            context.CrawlContext.CrawlCancelled
        );

        if( urls?.Any() is true )
        {
            foreach( var url in urls )
            {
                context.CrawlContext.UrlQueue.Enqueue( url );
            }
        }

        await next( context );
    }
}