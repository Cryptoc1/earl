namespace Earl.Crawler.Abstractions
{

    public interface IResultMetadataCollection : IReadOnlyList<object>
    {

        IReadOnlyList<T> GetOrderedMetadata<T>( )
            where T : class;

        T? GetMetadata<T>( )
            where T : class;

    }

}
