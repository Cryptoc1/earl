using System.Collections;
using Earl.Crawler.Middleware.Abstractions;

namespace Earl.Crawler.Middleware;

/// <summary> Default implementation of <see cref="ICrawlerFeatureCollection"/>. </summary>
/// <remarks> This implementation supports <see cref="IDisposable"/> and <see cref="IAsyncDisposable"/>, and will cascade disposal to any supporting features within the collection. </remarks>
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
        Dispose( true );
        GC.SuppressFinalize( this );
    }

    private void Dispose( bool disposing )
    {
        if( disposing )
        {
            if( features is not null )
            {
                foreach( object? feature in features.Values )
                {
                    if( feature is IDisposable disposable )
                    {
                        disposable.Dispose();
                    }
                }

                features = null;
            }
        }
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync( )
    {
        if( features is not null )
        {
            foreach( object? feature in features.Values )
            {
                if( feature is IAsyncDisposable asyncDisposable )
                {
                    await asyncDisposable.DisposeAsync().ConfigureAwait( false );
                }
                else if( feature is IDisposable disposable )
                {
                    disposable.Dispose();
                }
            }

            features = null;
        }

        Dispose( false );
        GC.SuppressFinalize( this );
    }

    /// <inheritdoc/>
    public TFeature? Get<TFeature>( )
        => ( TFeature? )this[ typeof( TFeature ) ];

    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<Type, object>> GetEnumerator( )
        => ( features ?? Enumerable.Empty<KeyValuePair<Type, object>>() )
            .GetEnumerator();

    /// <inheritdoc/>
    public void Set<TFeature>( TFeature? feature )
        => this[ typeof( TFeature ) ] = feature;

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator( )
        => GetEnumerator();
}