using Earl.Crawler.Abstractions.Events;
using Earl.Crawler.Middleware.Abstractions;

namespace Earl.Crawler.Events;

/// <summary> Extensions to <see cref="CrawlUrlContext"/> that supplement events. </summary>
public static class CrawlContextExtensions
{
    /// <summary> Emits the <see cref="CrawlProgressEvent"/> for the current state of the given <paramref name="context"/>. </summary>
    /// <param name="context"> The context to emit the <see cref="CrawlProgressEvent"/> for. </param>
    public static ValueTask OnProgressAsync( this CrawlContext context )
        => context.Options.Events.OnProgressAsync( new( context.TouchedUrls.Count, context.UrlQueue.Count, context.Services ), context.CrawlCancelled );
}