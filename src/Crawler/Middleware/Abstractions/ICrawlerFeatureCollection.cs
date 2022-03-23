namespace Earl.Crawler.Middleware.Abstractions;

/// <summary> Describes an arbitratry collection of objects by their types. </summary>
public interface ICrawlerFeatureCollection : IEnumerable<KeyValuePair<Type, object>>
{
    /// <summary> The integer version of the collection. </summary>
    /// <remarks> Incremented every mutation to the collection. </remarks>
    int Revision { get; }

    /// <summary> Retrieve a Feature by its <see cref="Type"/>. </summary>
    /// <param name="featureType"> The <see cref="Type"/> of the Feature to retrieve. </param>
    object? this[ Type featureType ] { get; set; }

    /// <summary> Retrieve a typed Feature, <typeparamref name="TFeature"/>. </summary>
    /// <typeparam name="TFeature"> The <see cref="Type"/> of the Feature to retrieve. </typeparam>
    TFeature? Get<TFeature>( );

    /// <summary> Set a typed Feature. </summary>
    /// <typeparam name="TFeature"> The <see cref="Type"/> of the Feature to set in the collection. </typeparam>
    /// <param name="feature"> The instance of <typeparamref name="TFeature"/> to add to the collection. </param>
    void Set<TFeature>( TFeature? feature );
}