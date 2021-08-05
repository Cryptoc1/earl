using System;
using System.Collections.Concurrent;
using System.Threading;
using Earl.Crawler.Abstractions;

namespace Earl.Crawler.Infrastructure.Abstractions
{

    public record CrawlContext(
        Uri Initiator,

        CancellationToken CrawlAborted,
        ICrawlOptions Options,
        ConcurrentDictionary<Uri, CrawlRequestResult?> Requests,
        ConcurrentQueue<Uri> UrlQueue
    );

}
