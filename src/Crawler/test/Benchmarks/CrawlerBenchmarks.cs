using Earl.Crawler.Abstractions;
using Earl.Crawler.Abstractions.Events;
using Earl.Crawler.Configuration;
using Earl.Crawler.Events.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Benchmarks;

public class CrawlerBenchmarks
{
    #region Fields
    private readonly IEarlCrawler crawler;
    private readonly Uri url;
    #endregion

    public CrawlerBenchmarks( )
    {
        crawler = new ServiceCollection()
            .AddEarlCrawler()
            .BuildServiceProvider()
            .GetRequiredService<IEarlCrawler>();

        url = new Uri( "https://webscraper.io/test-sites" );
    }

    [Benchmark]
    public Task Crawl( )
    {
        var options = CrawlerOptionsBuilder.CreateDefault()
            .Build();

        return crawler.CrawlAsync( url, options, CancellationToken.None );
    }

    [Benchmark]
    public Task CrawlWithHandlers( )
    {
        var options = CrawlerOptionsBuilder.CreateDefault()
            .On<CrawlErrorEvent>( OnError )
            .On<CrawlUrlStartedEvent>( OnUrlStarted )
            .On<CrawlUrlResultEvent>( OnUrlResult )
            .Build();

        return crawler.CrawlAsync( url, options, CancellationToken.None );
    }

    [Benchmark]
    public Task CrawlWithAsyncHandlers( )
    {
        var options = CrawlerOptionsBuilder.CreateDefault()
            .On<CrawlErrorEvent>( OnErrorAsync )
            .On<CrawlUrlStartedEvent>( OnUrlStartedAsync )
            .On<CrawlUrlResultEvent>( OnUrlResultAsync )
            .Build();

        return crawler.CrawlAsync( url, options, CancellationToken.None );
    }

    private static ValueTask OnError( CrawlErrorEvent e, CancellationToken cancellation )
        => ValueTask.CompletedTask;

    private static async ValueTask OnErrorAsync( CrawlErrorEvent e, CancellationToken cancellation )
        => await Task.Yield();

    private static ValueTask OnUrlStarted( CrawlUrlStartedEvent e, CancellationToken cancellation )
        => ValueTask.CompletedTask;

    private static async ValueTask OnUrlStartedAsync( CrawlUrlStartedEvent e, CancellationToken cancellation )
        => await Task.Yield();

    private static ValueTask OnUrlResult( CrawlUrlResultEvent e, CancellationToken cancellation )
        => ValueTask.CompletedTask;

    private static async ValueTask OnUrlResultAsync( CrawlUrlResultEvent e, CancellationToken cancellation )
        => await Task.Yield();
}