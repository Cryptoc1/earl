using Earl.Crawler.Abstractions.Events;
using Earl.Crawler.Events;
using Earl.Crawler.Middleware.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Middleware.Events;

/// <summary> Extensions to <see cref="CrawlUrlContext"/> that supplement events. </summary>
public static class CrawlContextExtensions
{
    /// <summary> Emits the <see cref="CrawlProgressEvent"/> for the current state of the given <paramref name="context"/>. </summary>
    /// <param name="context"> The context to emit the <see cref="CrawlProgressEvent"/> for. </param>
    public static async ValueTask OnProgressAsync( this CrawlContext context )
    {
        ArgumentNullException.ThrowIfNull( context );

        await using var scope = context.Services.CreateAsyncScope();
        await context.Options.Events.OnProgressAsync( new( context.TouchedUrls.Count, context.UrlQueue.Count, scope.ServiceProvider ), context.CrawlCancelled );
    }
}