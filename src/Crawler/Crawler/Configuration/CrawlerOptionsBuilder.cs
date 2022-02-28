using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Events;
using Earl.Crawler.Middleware.Html;
using Earl.Crawler.Middleware.Http;
using Earl.Crawler.Middleware.UrlScraping;

namespace Earl.Crawler.Configuration;

/// <summary> Default implementation of <see cref="ICrawlerOptionsBuilder"/>. </summary>
public class CrawlerOptionsBuilder : ICrawlerOptionsBuilder
{
    #region Properties

    /// <inheritdoc/>
    public IList<CrawlerOptionsBuildAction> BuildActions { get; } = new List<CrawlerOptionsBuildAction>();
    #endregion

    /// <inheritdoc/>
    public CrawlerOptions Build( )
    {
        var options = new CrawlerOptions(
            BatchDelay: null,
            BatchSize: Environment.ProcessorCount * 4,
            Events: new CrawlEvents(),
            MaxDegreeOfParallelism: Environment.ProcessorCount,
            MaxRequestCount: -1,
            Middleware: new List<ICrawlerMiddlewareDescriptor>(),
            Timeout: null
        );

        foreach( var buildAction in BuildActions )
        {
            options = buildAction( this, options );
        }

        return options;
    }

    /// <summary> Creates an instance of <see cref="CrawlerOptionsBuilder"/> with the default crawl configuration. </summary>
    public static ICrawlerOptionsBuilder CreateDefault( )
        => new CrawlerOptionsBuilder()
            .Use<HttpResponseMiddleware>()
            .Use<HtmlDocumentMiddleware>()
            .Use<UrlScraperMiddleware>();
}