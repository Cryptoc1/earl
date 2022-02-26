namespace Earl.Crawler.Configuration;

/// <summary> Extensions to <see cref="ICrawlerOptionsBuilder"/>. </summary>
public static class CrawlerOptionsBuilderExtensions
{
    /// <summary> Get the value for the given <paramref name="key"/> from the <see cref="ICrawlerOptionsBuilder.Properties"/>, or add the value created by the given <paramref name="factory"/>. </summary>
    /// <typeparam name="TKey"> The type of the key. </typeparam>
    /// <typeparam name="TValue"> The type of the value. </typeparam>
    /// <param name="builder"> The builder to retrieve the property value from. </param>
    /// <param name="key"> The key of the property to retrieve. </param>
    /// <param name="factory"> A factory method used to create the default value for the property if it does not exist. </param>
    public static TValue GetOrAddProperty<TKey, TValue>( this ICrawlerOptionsBuilder builder, TKey key, Func<TValue> factory )
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
}