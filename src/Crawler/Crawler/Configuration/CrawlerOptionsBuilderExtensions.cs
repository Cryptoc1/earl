namespace Earl.Crawler.Configuration;

/// <summary> Extensions to <see cref="ICrawlerOptionsBuilder"/>. </summary>
public static class CrawlerOptionsBuilderExtensions
{
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