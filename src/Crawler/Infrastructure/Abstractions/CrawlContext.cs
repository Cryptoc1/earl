using System;
using System.Collections.Concurrent;
using System.Threading;
using ConcurrentCollections;
using Earl.Crawler.Abstractions;

namespace Earl.Crawler.Infrastructure.Abstractions
{

    public record CrawlContext(
        Uri Initiator,

        CancellationToken CrawlAborted,
        ICrawlOptions Options,
        ConcurrentBag<CrawlRequestResult> Results,
        ConcurrentHashSet<Uri> TouchedUrls,
        ConcurrentQueue<Uri> UrlQueue
    );

}
