using Earl.Crawler.Abstractions;
using Earl.Crawler.Abstractions.Events;
using Earl.Crawler.Events;
using Earl.Crawler.Middleware.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Middleware.Events;

/// <summary> Extensions to <see cref="CrawlContext"/> that supplement events. </summary>
public static class CrawlUrlContextExtensions
{
    /// <summary> Emits the <see cref="CrawlErrorEvent"/> for the current state of the given <paramref name="context"/> and given <paramref name="exception"/>. </summary>
    /// <param name="context"> The context to emit the <see cref="CrawlErrorEvent"/> for. </param>
    /// <param name="exception"> The <see cref="Exception"/> to emit the <see cref="CrawlErrorEvent"/> for. </param>
    public static async ValueTask OnErrorAsync( this CrawlUrlContext context, Exception exception )
    {
        ArgumentNullException.ThrowIfNull( context );
        ArgumentNullException.ThrowIfNull( exception );

        await using var scope = context.Services.CreateAsyncScope();
        await context.CrawlContext.Options.Events.OnErrorAsync( new( exception, scope.ServiceProvider, context.Url ), context.CrawlContext.CrawlCancelled );
    }

    /// <summary> Emits the <see cref="CrawlProgressEvent"/> for the current state of the given <paramref name="context"/>. </summary>
    /// <param name="context"> The context to emit the <see cref="CrawlProgressEvent"/> for. </param>
    public static async ValueTask OnProgressAsync( this CrawlUrlContext context )
    {
        ArgumentNullException.ThrowIfNull( context );

        await using var scope = context.Services.CreateAsyncScope();
        await context.CrawlContext.Options.Events.OnProgressAsync(
           new( context.CrawlContext.TouchedUrls.Count, context.CrawlContext.UrlQueue.Count, scope.ServiceProvider, context.Url ),
           context.CrawlContext.CrawlCancelled
       );
    }

    /// <summary> Emits the <see cref="CrawlUrlResultEvent"/> for the current state of the given <paramref name="context"/> and given <paramref name="result"/>. </summary>
    /// <param name="context"> The context to emit the <see cref="CrawlUrlResultEvent"/> for. </param>
    /// <param name="result"> The <see cref="CrawlUrlResult"/> to emit the <see cref="CrawlUrlResultEvent"/> for. </param>
    public static async ValueTask OnResultAsync( this CrawlUrlContext context )
    {
        ArgumentNullException.ThrowIfNull( context );

        var result = context.Result.Build();
        await using var scope = context.Services.CreateAsyncScope();
        await context.CrawlContext.Options.Events.OnUrlResultAsync( new( result, scope.ServiceProvider ), context.CrawlContext.CrawlCancelled );
    }

    /// <summary> Emits the <see cref="CrawlUrlStartedEvent"/> for the current state of the given <paramref name="context"/>. </summary>
    /// <param name="context"> The context to emit the <see cref="CrawlUrlResultEvent"/> for. </param>
    public static async ValueTask OnStartedAsync( this CrawlUrlContext context )
    {
        ArgumentNullException.ThrowIfNull( context );

        await using var scope = context.Services.CreateAsyncScope();
        await context.CrawlContext.Options.Events.OnUrlStartedAsync( new( scope.ServiceProvider, context.Url ), context.CrawlContext.CrawlCancelled );
    }
}