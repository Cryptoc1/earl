using Earl.Crawler.Abstractions.Configuration;

namespace Earl.Crawler.Abstractions.Tests;

public sealed class ICrawlerOptionsBuilderExtensionsTests
{
    private static readonly Random Random = new();

    [Fact]
    public void BatchDelay_sets_batch_delay( )
    {
        var delay = TimeSpan.FromSeconds( Random.NextDouble() );
        var options = new OptionsBuilder()
            .BatchDelay( delay )
            .Build();

        Assert.Equal( delay, options.BatchDelay );
    }

    [Fact]
    public void BatchSize_sets_batch_size( )
    {
        int size = Random.Next();
        var options = new OptionsBuilder()
            .BatchSize( size )
            .Build();

        Assert.Equal( size, options.BatchSize );
    }

    [Fact]
    public void Configure_adds_build_action_to_builder( )
    {
        static CrawlerOptions action( ICrawlerOptionsBuilder _, CrawlerOptions options ) => options;
        var builder = new OptionsBuilder()
            .Configure( action );

        Assert.Collection( builder.BuildActions, a => Assert.Equal( action, a ) );
    }

    [Fact]
    public void MaxDegreeOfParallelism_sets_max_parallelism( )
    {
        int max = Random.Next();
        var options = new OptionsBuilder()
            .MaxDegreeOfParallelism( max )
            .Build();

        Assert.Equal( max, options.MaxDegreeOfParallelism );
    }

    [Fact]
    public void MaxRequestCount_sets_max_requests( )
    {
        int max = Random.Next();
        var options = new OptionsBuilder()
            .MaxRequestCount( max )
            .Build();

        Assert.Equal( max, options.MaxRequestCount );
    }

    [Fact]
    public void Timeout_sets_timeout( )
    {
        var timeout = TimeSpan.FromSeconds( Random.NextDouble() );
        var options = new OptionsBuilder()
            .Timeout( timeout )
            .Build();

        Assert.Equal( timeout, options.Timeout );
    }

    private sealed class OptionsBuilder : ICrawlerOptionsBuilder
    {
        public IList<CrawlerOptionsBuildAction> BuildActions { get; } = new List<CrawlerOptionsBuildAction>();

        public CrawlerOptions Build( )
        {
            var options = new CrawlerOptions( default, default, default!, default, default, default!, default );
            foreach( var action in BuildActions )
            {
                options = action( this, options );
            }

            return options;
        }
    }
}