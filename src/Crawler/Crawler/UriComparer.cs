namespace Earl.Crawler;

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

    /// <inheritdoc/>
    public int Compare( Uri? uri, Uri? anotherUri )
        => comparer.Compare( uri?.ToString(), anotherUri?.ToString() );

    /// <inheritdoc/>
    public bool Equals( Uri? uri, Uri? anotherUri )
        => comparer.Equals( uri?.ToString(), anotherUri?.ToString() );

    /// <inheritdoc/>
    public int GetHashCode( Uri uri )
    {
        if( uri is null )
        {
            throw new ArgumentNullException( nameof( uri ) );
        }

        return comparer.GetHashCode( uri.ToString() );
    }
}