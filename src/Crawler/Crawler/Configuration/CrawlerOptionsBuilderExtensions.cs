using Earl.Crawler.Abstractions.Configuration;

namespace Earl.Crawler.Configuration;

/// <summary> Extensions to <see cref="ICrawlerOptionsBuilder"/>. </summary>
public static class CrawlerOptionsBuilderExtensions
{
    /// <summary> Configure the <see cref="CrawlerOptions.BatchDelay"/>. </summary>
    /// <param name="builder"> The builder to configure. </param>
    /// <param name="delay"> The desired batch delay. </param>
    public static ICrawlerOptionsBuilder BatchDelay( this ICrawlerOptionsBuilder builder, TimeSpan delay )
    {
        ArgumentNullException.ThrowIfNull( builder );

        builder.Properties[ nameof( CrawlerOptions.BatchDelay ) ] = delay;
        return CrawlerOptionsBuilder.Decorate(
            builder,
            ( builder, options ) => options with { BatchDelay = builder.GetProperty<TimeSpan>( nameof( CrawlerOptions.BatchDelay ) ) }
        );
    }

    /// <summary> Configure the <see cref="CrawlerOptions.BatchSize"/>. </summary>
    /// <param name="builder"> The builder to configure. </param>
    /// <param name="size"> The desired batch size. </param>
    public static ICrawlerOptionsBuilder BatchSize( this ICrawlerOptionsBuilder builder, int size )
    {
        ArgumentNullException.ThrowIfNull( builder );

        builder.Properties[ nameof( CrawlerOptions.BatchSize ) ] = size;
        return CrawlerOptionsBuilder.Decorate(
            builder,
            ( builder, options ) => options with { BatchSize = builder.GetProperty<int>( nameof( CrawlerOptions.BatchSize ) ) }
        );
    }

    /// <summary> Get the <see cref="ICrawlerOptionsBuilder.Properties"/> value for the given <paramref name="key"/>, or add the value created by the given <paramref name="factory"/>. </summary>
    /// <typeparam name="TValue"> The type of the value. </typeparam>
    /// <param name="builder"> The builder to retrieve the property value from. </param>
    /// <param name="key"> The key of the property to retrieve. </param>
    /// <param name="factory"> A factory method used to create the default value for the property if it does not exist. </param>
    public static TValue GetOrAddProperty<TValue>( this ICrawlerOptionsBuilder builder, object key, Func<TValue> factory )
    {
        ArgumentNullException.ThrowIfNull( builder );
        ArgumentNullException.ThrowIfNull( key );

        if( builder.Properties.TryGetValue( key, out object? value ) && value is not null )
        {
            return ( TValue )value;
        }

        var typedValue = factory();
        builder.Properties[ key ] = typedValue;

        return typedValue;
    }

    /// <summary> Retrieve the <see cref="ICrawlerOptionsBuilder.Properties"/> value for the given <paramref name="key"/>, or <c>null</c> if it the <paramref name="key"/> doesn't exist. </summary>
    /// <typeparam name="TValue"> The type of the value. </typeparam>
    /// <param name="builder"> The builder to retrieve the property value from. </param>
    /// <param name="key"> The key of the property to retrieve. </param>
    public static TValue? GetProperty<TValue>( this ICrawlerOptionsBuilder builder, object key )
    {
        ArgumentNullException.ThrowIfNull( builder );
        ArgumentNullException.ThrowIfNull( key );

        return builder.Properties.TryGetValue( key, out object? value ) && value is not null
            ? ( TValue )value
            : default;
    }

    /// <summary> Configure the <see cref="CrawlerOptions.MaxDegreeOfParallelism"/>. </summary>
    /// <param name="builder"> The builder to configure. </param>
    /// <param name="max"> The desired maximum degree of parallelism. </param>
    public static ICrawlerOptionsBuilder MaxDegreeOfParallelism( this ICrawlerOptionsBuilder builder, int max )
    {
        ArgumentNullException.ThrowIfNull( builder );

        builder.Properties[ nameof( CrawlerOptions.MaxDegreeOfParallelism ) ] = max;
        return CrawlerOptionsBuilder.Decorate(
            builder,
            ( builder, options ) => options with { MaxDegreeOfParallelism = builder.GetProperty<int>( nameof( CrawlerOptions.MaxDegreeOfParallelism ) ) }
        );
    }

    /// <summary> Configure the <see cref="CrawlerOptions.MaxRequestCount"/>. </summary>
    /// <param name="builder"> The builder to configure. </param>
    /// <param name="max"> The desired maximum request count. </param>
    public static ICrawlerOptionsBuilder MaxRequestCount( this ICrawlerOptionsBuilder builder, int max )
    {
        ArgumentNullException.ThrowIfNull( builder );

        builder.Properties[ nameof( CrawlerOptions.MaxRequestCount ) ] = max;
        return CrawlerOptionsBuilder.Decorate(
            builder,
            ( builder, options ) => options with { MaxRequestCount = builder.GetProperty<int>( nameof( CrawlerOptions.MaxRequestCount ) ) }
        );
    }

    /// <summary> Configure the <see cref="CrawlerOptions.Timeout"/>. </summary>
    /// <param name="builder"> The builder to configure. </param>
    /// <param name="timeout"> The desired timeout. </param>
    public static ICrawlerOptionsBuilder Timeout( this ICrawlerOptionsBuilder builder, TimeSpan timeout )
    {
        ArgumentNullException.ThrowIfNull( builder );

        builder.Properties[ nameof( CrawlerOptions.Timeout ) ] = timeout;
        return CrawlerOptionsBuilder.Decorate(
            builder,
            ( builder, options ) => options with { Timeout = builder.GetProperty<TimeSpan?>( nameof( CrawlerOptions.Timeout ) ) }
        );
    }
}