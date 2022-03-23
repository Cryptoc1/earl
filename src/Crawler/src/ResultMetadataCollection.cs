using System.Collections;
using Earl.Crawler.Abstractions;

namespace Earl.Crawler;

/// <summary> Default implementation of <see cref="IResultMetadataCollection"/>. </summary>
public sealed class ResultMetadataCollection : IResultMetadataCollection
{
    /// <inheritdoc/>
    public object this[ int index ] => items[ index ];

    /// <inheritdoc/>
    public int Count => items.Length;

    private readonly object[] items;

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

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator( )
        => GetEnumerator();
}