namespace Earl.Crawler.Tests;

public sealed class UriComparerTests
{
    [Theory]
    [ClassData( typeof( UriComparisonData ) )]
    public void Comparer_compares_uris_with_string_comparer( StringComparer comparer, string url, string otherUrl, bool expected )
    {
        var uri = new Uri( url );
        var otherUri = new Uri( otherUrl );
        var uriComparer = new UriComparer( comparer );

        bool equal = ( ( uriComparer.Compare( uri, otherUri ) is 0 ) && uriComparer.Equals( uri, otherUri ) ) == expected;
        Assert.True( equal );
    }

    private sealed class UriComparisonData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator( )
        {
            yield return new object[] { StringComparer.CurrentCulture, "https://localhost/earl", "https://localhost/earl", true };
            yield return new object[] { StringComparer.CurrentCultureIgnoreCase, "https://localhost/earl", "https://localhost/Earl", true };
            yield return new object[] { StringComparer.InvariantCulture, "https://localhost/earl", "https://localhost/earl", true };
            yield return new object[] { StringComparer.InvariantCulture, "https://localhost/earl", "https://localhost/Earl", false };
            yield return new object[] { StringComparer.InvariantCulture, "https://localhost/earl", "https://localhost/Earl", false };
            yield return new object[] { StringComparer.InvariantCultureIgnoreCase, "https://localhost/earl", "https://localhost/Earl", true };
            yield return new object[] { StringComparer.InvariantCultureIgnoreCase, "https://localhost/earl", "https://localhost/earl", true };
            yield return new object[] { StringComparer.InvariantCultureIgnoreCase, "https://localhost/earl", "https://localhost/earl", true };
            yield return new object[] { StringComparer.Ordinal, "https://localhost/earl", "https://localhost/earl", true };
            yield return new object[] { StringComparer.Ordinal, "https://localhost/earl", "https://localhost/Earl", false };
            yield return new object[] { StringComparer.OrdinalIgnoreCase, "https://localhost/earl", "https://localhost/Earl", true };
            yield return new object[] { StringComparer.OrdinalIgnoreCase, "https://localhost/earl", "https://localhost/earl", true };
        }

        IEnumerator IEnumerable.GetEnumerator( ) => GetEnumerator();
    }
}