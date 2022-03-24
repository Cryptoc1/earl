using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Middleware.Abstractions;

namespace Earl.Crawler.Tests;

public sealed class CrawlContextExtensionsTests
{
    [Theory]
    [InlineData( 5, 0, 0, 5 )]
    [InlineData( 5, 25, 22, 3 )]
    [InlineData( 5, 25, 25, 0 )]
    [InlineData( 25, 5, 0, 5 )]
    public void Context_calculates_effective_batch_size( int batchSize, int maxRequestSize, int touchedCount, int expected )
    {
        var options = new CrawlerOptions( default, batchSize, default!, default, maxRequestSize, default!, default );
        var context = new CrawlContext(
            default!,
            CancellationToken.None,
            options,
            default!,
            new( Enumerable.Range( 0, touchedCount ).Select( _ => new Uri( $"https://localhost:8080/{_}" ) ) ),
            default!
        );

        int size = context.GetEffectiveBatchSize();
        Assert.Equal( expected, size );
    }

    [Theory]
    [InlineData( 0, 0, false )]
    [InlineData( 0, 10, false )]
    [InlineData( 5, 0, false )]
    [InlineData( 5, 10, true )]
    public void Context_determines_whether_max_requests_has_been_exceeded( int maxRequestSize, int touchedCount, bool expected )
    {
        var options = new CrawlerOptions( default, default, default!, default, maxRequestSize, default!, default );
        var context = new CrawlContext(
            default!,
            CancellationToken.None,
            options,
            default!,
            new( Enumerable.Range( 0, touchedCount ).Select( _ => new Uri( $"https://localhost:8080/{_}" ) ) ),
            default!
        );

        bool exceeded = context.HasExceededRequests();
        Assert.Equal( expected, exceeded );
    }
}