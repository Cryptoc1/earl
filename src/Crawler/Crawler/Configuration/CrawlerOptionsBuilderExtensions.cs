namespace Earl.Crawler.Configuration;

/// <summary> Extensions to <see cref="ICrawlerOptionsBuilder"/>. </summary>
public static class CrawlerOptionsBuilderExtensions
{
    public static ICrawlerOptionsBuilder BatchSize( this ICrawlerOptionsBuilder builder, int size )
    {
        ArgumentNullException.ThrowIfNull( builder );

        builder.Properties[ nameof( BatchSize ) ] = size;
        return CrawlerOptionsBuilder.Decorate(
            builder,
            ( builder, options ) => options with { BatchSize = builder.GetProperty<int>( nameof( BatchSize ) ) }
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

    public static ICrawlerOptionsBuilder Timeout( this ICrawlerOptionsBuilder builder, TimeSpan timeout )
    {
        ArgumentNullException.ThrowIfNull( builder );

        builder.Properties[ nameof( Timeout ) ] = timeout;
        return CrawlerOptionsBuilder.Decorate(
            builder,
            ( builder, options ) => options with { Timeout = builder.GetProperty<TimeSpan?>( nameof( Timeout ) ) }
        );
    }
}