namespace Earl.Crawler.Middleware.UrlScraping.Abstractions.Configuration;

/// <summary> Extensions to <see cref="UrlScraperOptions"/>. </summary>
public static class UrlScraperOptionsExtensions
{
    /// <summary> Register the <see cref="IUrlFilter"/> of type <typeparamref name="TFilter"/>. </summary>
    /// <typeparam name="TFilter"> The type of <see cref="IUrlFilter"/> to register. </typeparam>
    /// <param name="options"> The <see cref="UrlScraperOptions"/> to register the filter to. </param>
    public static UrlScraperOptions WithFilter<TFilter>( this UrlScraperOptions options )
        where TFilter : IUrlFilter
    {
        ArgumentNullException.ThrowIfNull( options );
        return options with
        {
            Filters = options.Filters
                .Append( new ServiceUrlFilterDescriptor( typeof( TFilter ) ) )
                .ToList()
        };
    }
}