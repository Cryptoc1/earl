using System.Collections;
using Earl.Crawler.Middleware.Abstractions;

namespace Earl.Crawler
{

    public sealed class CrawlerFeatureCollection : ICrawlerFeatureCollection, IDisposable, IAsyncDisposable
    {
        #region Fields
        private IDictionary<Type, object>? features;
        private int revision = -1;
        #endregion

        #region Properties

        /// <inheritdoc/>
        public int Revision => revision;
        #endregion

        /// <inheritdoc/>
        public object? this[ Type key ]
        {
            get
            {
                if( key is null )
                {
                    throw new ArgumentNullException( nameof( key ) );
                }

                return features?.TryGetValue( key, out var value ) is true ? value : null;
            }
            set
            {
                if( key is null )
                {
                    throw new ArgumentNullException( nameof( key ) );
                }

                if( value is null )
                {
                    if( features is not null && features.Remove( key ) )
                    {
                        revision++;
                    }

                    return;
                }

                if( features is null )
                {
                    features = new Dictionary<Type, object>();
                    revision = 0;
                }

                features[ key ] = value;
                revision++;
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

}
