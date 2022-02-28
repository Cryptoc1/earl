using Earl.Agent.Console.Abstractions;
using Earl.Crawler.Abstractions;
using Earl.Crawler.Abstractions.Events;
using Earl.Crawler.Configuration;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Earl.Agent.Console;

public class DefaultCommand : CancellableAsyncCommand
{
    #region Fields
    private readonly IEarlCrawler crawler;
    #endregion

    public DefaultCommand( IEarlCrawler crawler )
        => this.crawler = crawler;

    /// <inheritdoc/>
    public override async Task<int> ExecuteAsync( CommandContext context, CancellationToken cancellation )
    {
        ArgumentNullException.ThrowIfNull( context );

        await AnsiConsole.Status()
            .StartAsync(
                $"Initiating crawl",
                async context =>
                {
                    await Task.Delay( 1000 );

                    int count = 0;

                    var url = new Uri( "https://webscraper.io/" );
                    var options = CrawlerOptionsBuilder.CreateDefault()
                        .On<CrawlErrorEvent>( onError )
                        .On<CrawlUrlResultEvent>( onUrlResult )
                        .On<CrawlUrlStartedEvent>( onUrlStarted )
                        .Build();

                    var crawl = crawler.CrawlAsync( url, options, cancellation );

                    context.SpinnerStyle( Style.Parse( "green bold" ) );
                    context.Spinner( Spinner.Known.Triangle );
                    context.Status( $"Crawling '{url}'" );

                    await Task.Delay( 1500 );
                    await crawl;

                    return;

                    ValueTask onError( CrawlErrorEvent e, CancellationToken cancellation )
                    {
                        if( e.Url is not null )
                        {
                            AnsiConsole.MarkupLine( $"[red]![/] {e.Url}" );
                        }

                        return ValueTask.CompletedTask;
                    }

                    ValueTask onUrlStarted( CrawlUrlStartedEvent e, CancellationToken cancellation )
                    {
                        context.Status( $"Crawling '{e.Url}'" );
                        return ValueTask.CompletedTask;
                    }

                    ValueTask onUrlResult( CrawlUrlResultEvent e, CancellationToken cancellation )
                    {
                        AnsiConsole.MarkupLine( $"[green]✔[/] {e.Result.Url}" );
                        Interlocked.Increment( ref count );

                        return ValueTask.CompletedTask;
                    }
                }
           );

        return 0;
    }
}