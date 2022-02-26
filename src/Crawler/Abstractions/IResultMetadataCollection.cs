namespace Earl.Crawler.Abstractions;

/// <summary> Describes an arbitrary collection of objects representing the data of a crawl result. </summary>
public interface IResultMetadataCollection : IReadOnlyList<object>
{
    /// <summary> Retrieve all objects of the specified type <typeparamref name="T"/>. </summary>
    /// <typeparam name="T"> The type of the metadata objects to retrieve. </typeparam>
    IReadOnlyList<T> GetOrderedMetadata<T>( )
        where T : class;

    /// <summary> Retrieve the effective object of the specified type <typeparamref name="T"/>. </summary>
    /// <typeparam name="T"> The type of the metadata objects to retrieve. </typeparam>
    T? GetMetadata<T>( )
        where T : class;
}