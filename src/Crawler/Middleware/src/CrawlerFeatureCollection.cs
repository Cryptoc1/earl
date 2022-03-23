using System.Collections;
using Earl.Crawler.Middleware.Abstractions;

namespace Earl.Crawler.Middleware;

/// <summary> Default implementation of <see cref="ICrawlerFeatureCollection"/>. </summary>
public sealed class CrawlerFeatureCollection : ICrawlerFeatureCollection, IDisposable, IAsyncDisposable
{
    /// <inheritdoc/>
    public int Revision { get; private set; } = -1;

    private IDictionary<Type, object>? features;

    /// <inheritdoc/>
    public object? this[ Type key ]
    {
        get
        {
            ArgumentNullException.ThrowIfNull( key );
            return features?.TryGetValue( key!, out object? value ) is true ? value : null;
        }

        set
        {
            ArgumentNullException.ThrowIfNull( key );
            if( value is null )
            {
                if( features is not null && features.Remove( key ) )
                {
                    Revision++;
                }

                return;
            }

            if( features is null )
            {
                features = new Dictionary<Type, object>();
                Revision = 0;
            }

            features[ key ] = value;
            Revision++;
        }
    }

    /// <inheritdoc/>
    public void Dispose( )
    {
        if( features is not null )
        {
            var disposables = features.Values.Where( feature => feature is IDisposable )
                .Select( feature => feature as IDisposable );

            foreach( var feature in disposables )
            {
                feature!.Dispose();
            }
        }
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync( )
    {
        if( features is not null )
        {
            var disposables = features.Values.Where( feature => feature is IAsyncDisposable )
                .Select( feature => feature as IAsyncDisposable );

            foreach( var feature in disposables )
            {
                await feature!.DisposeAsync();
            }
        }
    }

    /// <inheritdoc/>
    public TFeature? Get<TFeature>( )
        => ( TFeature? )this[ typeof( TFeature ) ];

    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<Type, object>> GetEnumerator( )
        => ( features ?? Enumerable.Empty<KeyValuePair<Type, object>>() )
            .GetEnumerator();

    /// <inheritdoc/>
    public void Set<TFeature>( TFeature? instance )
        => this[ typeof( TFeature ) ] = instance;

    IEnumerator IEnumerable.GetEnumerator( )
        => GetEnumerator();
}