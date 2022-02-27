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
                    await Task.Delay( 500 );

                    var url = new Uri( "https://webscraper.io/test-sites/e-commerce/static" );
                    var options = CrawlerOptionsBuilder.CreateDefault()
                        .On<CrawlErrorEvent>( onError )
                        .On<CrawlUrlResultEvent>( onUrlResult )
                        .Build();

                    var crawl = crawler.CrawlAsync( url, options, cancellation );

                    context.SpinnerStyle( Style.Parse( "green bold" ) );
                    context.Spinner( Spinner.Known.Triangle );
                    context.Status( $"Crawling" );

                    await Task.Delay( 1500 );
                    await crawl;

                    static Task onError( CrawlErrorEvent e, CancellationToken cancellation )
                    {
                        if( e.Url is not null )
                        {
                            AnsiConsole.MarkupLine( $"[red]![/] {e.Url}" );
                        }

                        return Task.CompletedTask;
                    }

                    static Task onUrlResult( CrawlUrlResultEvent e, CancellationToken cancellation )
                    {
                        AnsiConsole.MarkupLine( $"[green]✔[/] {e.Result.Url}" );
                        return Task.CompletedTask;
                    }
                }
            );

        return 0;
    }
}