using System;
using System.Collections.Concurrent;
using System.Threading;
using Earl.Crawler.Abstractions;

namespace Earl.Crawler.Infrastructure.Abstractions
{

    /// <summary> Represents the state of a processing crawl. </summary>
    /// <param name="Initiator"> The url that initiated the crawl. </param>
    /// <param name="CrawlAborted"> A <see cref="CancellationToken"/> that triggers when the crawl is cancelled. </param>
    /// <param name="Options"> The <see cref="ICrawlOptions"/> that initiated the crawl. </param>
    /// <param name="Requests"> Collection of processing requests. </param>
    /// <param name="UrlQueue"> Collection of urls to be crawled. </param>
    public record CrawlContext(
        Uri Initiator,

        CancellationToken CrawlAborted,
        ICrawlOptions Options,
        ConcurrentDictionary<Uri, CrawlRequestResult?> Requests,
        ConcurrentQueue<Uri> UrlQueue
    );

}
