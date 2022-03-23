namespace Earl.Crawler.Middleware.Tests;

public sealed class CrawlerFeatureCollectionTests
{
    [Fact]
    public void Collection_disposes_features( )
    {
        using var feature = new TestFeature();
        using( var features = new CrawlerFeatureCollection() )
        {
            features.Set<ITestFeature>( feature );
        }

        Assert.True( feature.Disposed );
    }

    [Fact]
    public async ValueTask Collection_disposes_features_async( )
    {
        await using var feature = new TestFeature();
        await using( var features = new CrawlerFeatureCollection() )
        {
            features.Set<ITestFeature>( feature );
        }

        Assert.True( feature.Disposed );
    }

    [Fact]
    public void Collection_increments_revision_on_mutation( )
    {
        var features = new CrawlerFeatureCollection();

        features.Set<ITestFeature>( new TestFeature() );
        Assert.Equal( 1, features.Revision );

        features.Set<ITestFeature>( null );
        Assert.Equal( 2, features.Revision );
    }

    [Fact]
    public void Collection_sets_feature( )
    {
        var features = new CrawlerFeatureCollection();
        var feature = new TestFeature();

        features.Set<ITestFeature>( feature );
        Assert.Equal( feature, features.Get<ITestFeature>() );
    }

    private interface ITestFeature
    {
    }

    private sealed class TestFeature : ITestFeature, IAsyncDisposable, IDisposable
    {
        public bool Disposed { get; private set; } = false;

        public void Dispose( ) => Dispose( true );

        private void Dispose( bool disposing )
        {
            if( disposing )
            {
                Disposed = true;
            }
        }

        public ValueTask DisposeAsync( )
        {
            Disposed = true;

            Dispose( false );
            return default;
        }
    }
}