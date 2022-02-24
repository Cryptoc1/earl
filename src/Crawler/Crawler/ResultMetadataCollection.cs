using System.Collections;
using Earl.Crawler.Abstractions;

namespace Earl.Crawler;

public class ResultMetadataCollection : IResultMetadataCollection
{
    #region Fields
    private readonly object[] items;
    #endregion

    #region Properties
    /// <inheritdoc/>
    public object this[ int index ] => items[ index ];

    /// <inheritdoc/>
    public int Count => items.Length;
    #endregion

    public ResultMetadataCollection( IEnumerable<object> items )
        => this.items = items.ToArray();

    /// <inheritdoc/>
    public IEnumerator<object> GetEnumerator( )
        => ( ( IEnumerable<object> )items ).GetEnumerator();

    /// <inheritdoc/>
    public T? GetMetadata<T>( )
        where T : class
        => throw new NotImplementedException();

    /// <inheritdoc/>
    public IReadOnlyList<T> GetOrderedMetadata<T>( )
        where T : class
        => throw new NotImplementedException();

    IEnumerator IEnumerable.GetEnumerator( )
        => GetEnumerator();
}