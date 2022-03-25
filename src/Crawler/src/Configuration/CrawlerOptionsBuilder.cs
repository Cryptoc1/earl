using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Events;
using Earl.Crawler.Middleware.Configuration;
using Earl.Crawler.Middleware.Html;
using Earl.Crawler.Middleware.Http;
using Earl.Crawler.Middleware.UrlScraping.Configuration;

namespace Earl.Crawler.Configuration;

/// <summary> Default implementation of <see cref="ICrawlerOptionsBuilder"/>. </summary>
public sealed class CrawlerOptionsBuilder : ICrawlerOptionsBuilder
{
    /// <inheritdoc/>
    public IList<CrawlerOptionsBuildAction> BuildActions { get; } = new List<CrawlerOptionsBuildAction>();

    private CrawlerOptionsBuilder( )
    {
    }

    /// <inheritdoc/>
    public CrawlerOptions Build( )
    {
        var options = new CrawlerOptions(
            BatchDelay: null,
            BatchSize: ( int )Math.Pow( Environment.ProcessorCount, 2 ),
            Events: CrawlerEvents.Empty,
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
            .UseUrlScraper();
}