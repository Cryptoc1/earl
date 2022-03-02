using Earl.Crawler.Abstractions;
using Earl.Crawler.Abstractions.Events;
using Earl.Crawler.Middleware.Abstractions;

namespace Earl.Crawler.Events;

/// <summary> Extensions to <see cref="CrawlContext"/> that supplement events. </summary>
public static class CrawlUrlContextExtensions
{
    /// <summary> Emits the <see cref="CrawlErrorEvent"/> for the current state of the given <paramref name="context"/> and given <paramref name="exception"/>. </summary>
    /// <param name="context"> The context to emit the <see cref="CrawlErrorEvent"/> for. </param>
    /// <param name="exception"> The <see cref="Exception"/> to emit the <see cref="CrawlErrorEvent"/> for. </param>
    public static ValueTask OnErrorAsync( this CrawlUrlContext context, Exception exception )
        => context.CrawlContext.Options.Events.OnErrorAsync( new( exception, context.Services, context.Url ), context.CrawlContext.CrawlCancelled );

    /// <summary> Emits the <see cref="CrawlProgressEvent"/> for the current state of the given <paramref name="context"/>. </summary>
    /// <param name="context"> The context to emit the <see cref="CrawlProgressEvent"/> for. </param>
    public static ValueTask OnProgressAsync( this CrawlUrlContext context )
        => context.CrawlContext.Options.Events.OnProgressAsync(
            new( context.CrawlContext.TouchedUrls.Count, context.CrawlContext.UrlQueue.Count, context.Services, context.Url ),
            context.CrawlContext.CrawlCancelled
        );

    /// <summary> Emits the <see cref="CrawlUrlResultEvent"/> for the current state of the given <paramref name="context"/> and given <paramref name="result"/>. </summary>
    /// <param name="context"> The context to emit the <see cref="CrawlUrlResultEvent"/> for. </param>
    /// <param name="result"> The <see cref="CrawlUrlResult"/> to emit the <see cref="CrawlUrlResultEvent"/> for. </param>
    public static ValueTask OnResultAsync( this CrawlUrlContext context, CrawlUrlResult result )
        => context.CrawlContext.Options.Events.OnUrlResultAsync( new( result, context.Services ), context.CrawlContext.CrawlCancelled );

    /// <summary> Emits the <see cref="CrawlUrlStartedEvent"/> for the current state of the given <paramref name="context"/>. </summary>
    public static ValueTask OnStartedAsync( this CrawlUrlContext context )
        => context.CrawlContext.Options.Events.OnUrlStartedAsync( new( context.Services, context.Url ), context.CrawlContext.CrawlCancelled );
}