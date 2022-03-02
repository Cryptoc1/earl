namespace Earl.Crawler.Abstractions.Configuration;

/// <summary> Extensions to <see cref="ICrawlerOptionsBuilder"/>. </summary>
public static class ICrawlerOptionsBuilderExtensions
{
    /// <summary> Configure the <see cref="CrawlerOptions.BatchDelay"/>. </summary>
    /// <param name="builder"> The builder to configure. </param>
    /// <param name="delay"> The desired batch delay. </param>
    public static ICrawlerOptionsBuilder BatchDelay( this ICrawlerOptionsBuilder builder, TimeSpan delay )
    {
        ArgumentNullException.ThrowIfNull( builder );
        return builder.Configure(
            ( _, options ) => options with { BatchDelay = delay }
        );
    }

    /// <summary> Configure the <see cref="CrawlerOptions.BatchSize"/>. </summary>
    /// <param name="builder"> The builder to configure. </param>
    /// <param name="size"> The desired batch size. </param>
    public static ICrawlerOptionsBuilder BatchSize( this ICrawlerOptionsBuilder builder, int size )
    {
        ArgumentNullException.ThrowIfNull( builder );
        return builder.Configure(
            ( _, options ) => options with { BatchSize = size }
        );
    }

    /// <summary> Adds the given build action to the builder. </summary>
    /// <param name="builder"> The builder to add the <paramref name="configure"/> to. </param>
    /// <param name="configure"> The <see cref="CrawlerOptionsBuildAction"/> to add. </param>
    public static ICrawlerOptionsBuilder Configure( this ICrawlerOptionsBuilder builder, CrawlerOptionsBuildAction configure )
    {
        ArgumentNullException.ThrowIfNull( builder );
        ArgumentNullException.ThrowIfNull( configure );

        builder.BuildActions.Add( configure );
        return builder;
    }

    /// <summary> Configure the <see cref="CrawlerOptions.MaxDegreeOfParallelism"/>. </summary>
    /// <param name="builder"> The builder to configure. </param>
    /// <param name="maxParallelism"> The desired maximum degree of parallelism. </param>
    public static ICrawlerOptionsBuilder MaxDegreeOfParallelism( this ICrawlerOptionsBuilder builder, int maxParallelism )
    {
        ArgumentNullException.ThrowIfNull( builder );
        return builder.Configure(
            ( _, options ) => options with { MaxDegreeOfParallelism = maxParallelism }
        );
    }

    /// <summary> Configure the <see cref="CrawlerOptions.MaxRequestCount"/>. </summary>
    /// <param name="builder"> The builder to configure. </param>
    /// <param name="maxRequests"> The desired maximum request count. </param>
    public static ICrawlerOptionsBuilder MaxRequestCount( this ICrawlerOptionsBuilder builder, int maxRequests )
    {
        ArgumentNullException.ThrowIfNull( builder );
        return builder.Configure(
            ( _, options ) => options with { MaxRequestCount = maxRequests }
        );
    }

    /// <summary> Configure the <see cref="CrawlerOptions.Timeout"/>. </summary>
    /// <param name="builder"> The builder to configure. </param>
    /// <param name="timeout"> The desired timeout. </param>
    public static ICrawlerOptionsBuilder Timeout( this ICrawlerOptionsBuilder builder, TimeSpan timeout )
    {
        ArgumentNullException.ThrowIfNull( builder );
        return builder.Configure(
            ( _, options ) => options with { Timeout = timeout }
        );
    }
}