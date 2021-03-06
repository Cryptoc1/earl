using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Middleware.Abstractions;

namespace Earl.Crawler;

/// <summary> Extensions to <see cref="CrawlContext"/>. </summary>
public static class CrawlContextExtensions
{
    /// <summary> Calculate the batch size for the given <paramref name="context"/>. </summary>
    /// <param name="context"> The <see cref="CrawlContext"/> to evaluate. </param>
    /// <returns> The <see cref="CrawlerOptions.BatchSize"/> for the given <paramref name="context"/>, or if a <see cref="CrawlerOptions.MaxRequestCount"/> is specified, the difference of the <see cref="CrawlerOptions.MaxRequestCount"/> and the number of <see cref="CrawlContext.TouchedUrls"/>. </returns>
    public static int GetEffectiveBatchSize( this CrawlContext context )
    {
        int size = Math.Max( 1, context.Options.BatchSize );
        if( context.Options.MaxRequestCount > 0 )
        {
            int capacity = context.Options.MaxRequestCount - context.TouchedUrls.Count;
            return Math.Max( 0, Math.Min( size, capacity ) );
        }

        return size;
    }

    /// <summary> Determine whether the number <see cref="CrawlContext.TouchedUrls"/> has exceeded the <see cref="CrawlerOptions.MaxRequestCount"/> for the given <paramref name="context"/>. </summary>
    /// <param name="context"> The <see cref="CrawlContext"/> to evaluate. </param>
    public static bool HasExceededRequests( this CrawlContext context )
        => context.Options.MaxRequestCount > 0 && context.TouchedUrls.Count >= context.Options.MaxRequestCount;
}