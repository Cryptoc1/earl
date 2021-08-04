using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Earl.Crawler.Infrastructure.Abstractions;

namespace Earl.Crawler
{

    public sealed class CrawlRequestFeatureCollection : ICrawlRequestFeatureCollection, IDisposable, IAsyncDisposable
    {
        #region Fields
        private IDictionary<Type, object>? features;
        private int revision = -1;
        #endregion

        #region Properties
        public int Revision => revision;
        #endregion

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

                if( features == null )
                {
                    features = new Dictionary<Type, object>();
                    revision = 0;
                }

                features[ key ] = value;
                revision++;
            }
        }

        public void Dispose( )
        {
            if( features is not null )
            {
                var disposables = features.Values.Where( feature => feature is IDisposable )
                    .Select( feature => feature as IDisposable );

                foreach( var feature in disposables )
                {
                    feature.Dispose();
                }
            }
        }

        public async ValueTask DisposeAsync( )
        {
            if( features is not null )
            {
                var disposables = features.Values.Where( feature => feature is IAsyncDisposable )
                    .Select( feature => feature as IAsyncDisposable );

                foreach( var feature in disposables )
                {
                    await feature.DisposeAsync();
                }
            }
        }

        public TFeature? Get<TFeature>( )
            => ( TFeature? )this[ typeof( TFeature ) ];

        public IEnumerator<KeyValuePair<Type, object>> GetEnumerator( )
            => features?.GetEnumerator();

        public void Set<TFeature>( TFeature? instance )
            => this[ typeof( TFeature ) ] = instance;

        IEnumerator IEnumerable.GetEnumerator( )
            => GetEnumerator();

    }

}
