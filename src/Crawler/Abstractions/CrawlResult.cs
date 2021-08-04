using System;
using System.Collections.Generic;

namespace Earl.Crawler.Abstractions
{

    public record CrawlResult
    (
        Uri Initiator,
        IReadOnlyCollection<CrawlRequestResult> Results
    );

}
