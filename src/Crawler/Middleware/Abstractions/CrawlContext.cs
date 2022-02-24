using System.Collections.Concurrent;
using Axion.Collections.Concurrent;
using Earl.Crawler.Abstractions;

namespace Earl.Crawler.Middleware.Abstractions;

/// <summary> Represents the state of a processing crawl. </summary>
/// <param name="Initiator"> The url that initiated the crawl. </param>
/// <param name="CrawlAborted"> A <see cref="CancellationToken"/> that triggers when the crawl is cancelled. </param>
/// <param name="Options"> The <see cref="ICrawlOptions"/> that initiated the crawl. </param>
/// <param name="TouchedUrls"> Collection of processing requests. </param>
/// <param name="UrlQueue"> Collection of urls to be crawled. </param>
public record CrawlContext(
    Uri Initiator,

    CancellationToken CrawlAborted,
    ICrawlOptions Options,
    ConcurrentHashSet<Uri> TouchedUrls,
    ConcurrentQueue<Uri> UrlQueue
);