using System;
using System.Collections.Generic;

namespace Earl.Crawler.Infrastructure.Abstractions
{
    public interface ICrawlRequestFeatureCollection : IEnumerable<KeyValuePair<Type, object>>
    {

        #region Properties
        int Revision { get; }
        #endregion

        object? this[ Type key ] { get; set; }

        TFeature? Get<TFeature>( );

        void Set<TFeature>( TFeature? instance );

    }
}
