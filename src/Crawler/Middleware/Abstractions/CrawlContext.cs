using System.Collections.Concurrent;
using Axion.Collections.Concurrent;
using Earl.Crawler.Abstractions.Configuration;

namespace Earl.Crawler.Middleware.Abstractions;

/// <summary> Represents the state of a processing crawl. </summary>
/// <param name="Initiator"> The url that initiated the crawl. </param>
/// <param name="CrawlCancelled"> A <see cref="CancellationToken"/> that triggers when the crawl is cancelled. </param>
/// <param name="Options"> The <see cref="CrawlerOptions"/> that initiated the crawl. </param>
/// <param name="Services"> An <see cref="IServiceProvider"/> that exposes services available to the context. </param>
/// <param name="TouchedUrls"> Collection of processing requests. </param>
/// <param name="UrlQueue"> Collection of urls to be crawled. </param>
public record CrawlContext(
    Uri Initiator,

    CancellationToken CrawlCancelled,
    CrawlerOptions Options,
    IServiceProvider Services,
    ConcurrentHashSet<Uri> TouchedUrls,
    ConcurrentQueue<Uri> UrlQueue
);