namespace Earl.Crawler.Abstractions;

/// <summary> A type that can determine the equality of <see cref="Uri"/>s' via their string value. </summary>
public sealed class UriComparer : IComparer<Uri>, IEqualityComparer<Uri>
{
    /// <summary> An <see cref="UriComparer"/> derived from <see cref="StringComparer.OrdinalIgnoreCase"/>. </summary>
    public static UriComparer OrdinalIgnoreCase { get; } = new( StringComparer.OrdinalIgnoreCase );

    private readonly StringComparer comparer;

    public UriComparer( StringComparer comparer )
        => this.comparer = comparer;

    /// <inheritdoc/>
    public int Compare( Uri? uri, Uri? anotherUri )
        => comparer.Compare( uri?.ToString(), anotherUri?.ToString() );

    /// <inheritdoc/>
    public bool Equals( Uri? uri, Uri? anotherUri )
        => comparer.Equals( uri?.ToString(), anotherUri?.ToString() );

    /// <inheritdoc/>
    public int GetHashCode( Uri uri )
    {
        ArgumentNullException.ThrowIfNull( uri );
        return comparer.GetHashCode( uri.ToString() );
    }
}