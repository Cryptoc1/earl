using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Earl.Crawler
{
    public class UriComparer : IComparer<Uri>, IEqualityComparer<Uri>
    {
        #region Fields
        private readonly StringComparer comparer;
        #endregion

        #region Properties
        public static UriComparer OrdinalIgnoreCase = new( StringComparer.OrdinalIgnoreCase );
        #endregion

        public UriComparer( StringComparer comparer )
            => this.comparer = comparer;

        public int Compare( Uri? uri, Uri? anotherUri )
            => comparer.Compare( uri?.ToString(), anotherUri?.ToString() );

        public bool Equals( Uri? uri, Uri? anotherUri )
            => comparer.Equals( uri?.ToString(), anotherUri?.ToString() );

        public int GetHashCode( [DisallowNull] Uri uri )
           => comparer.GetHashCode( uri.ToString() );

    }
}
