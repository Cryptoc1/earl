using System;

namespace Earl.Crawler.Infrastructure.Abstractions
{

    public record CrawlRequestContext
    (
        CrawlContext CrawlContext,
        ICrawlRequestFeatureCollection Features,
        Guid Id,
        IServiceProvider Services,
        Uri Url
    );

}
