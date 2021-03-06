using System.ComponentModel;
using Earl.Agent.Console.Abstractions;
using Earl.Crawler.Abstractions;
using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Abstractions.Events;
using Earl.Crawler.Configuration;
using Earl.Crawler.Events.Configuration;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Earl.Agent.Console;

public sealed class DefaultCommand : CancellableAsyncCommand<DefaultCommandSettings>
{
    private readonly IEarlCrawler crawler;

    public DefaultCommand( IEarlCrawler crawler )
        => this.crawler = crawler;

    /// <inheritdoc/>
    public override async Task<int> ExecuteAsync( CommandContext context, DefaultCommandSettings settings, CancellationToken cancellation )
    {
        ArgumentNullException.ThrowIfNull( context );

        int count = 0;
        await AnsiConsole.Status()
            .StartAsync(
                $"Initiating crawl",
                async context =>
                {
                    await Task.Delay( 1250 );

                    var builder = CrawlerOptionsBuilder.CreateDefault()
                        .On<CrawlErrorEvent>( onError )
                        .On<CrawlUrlResultEvent>( onUrlResult )
                        .On<CrawlUrlStartedEvent>( onUrlStarted );
                    /* .PersistTo(
                        persist => persist.ToJson(
                            json => json.Destination( Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.Desktop ), "CrawlResults" ) )
                        )
                    ); */

                    if( settings.BatchDelay.HasValue )
                    {
                        builder.BatchDelay( settings.BatchDelay.Value );
                    }

                    if( settings.BatchSize.HasValue )
                    {
                        builder.BatchSize( settings.BatchSize.Value );
                    }

                    var options = builder.Build();
                    var crawl = crawler.CrawlAsync( settings.Url, options, cancellation );

                    context.SpinnerStyle( Style.Parse( "green bold" ) );
                    context.Spinner( Spinner.Known.Triangle );
                    context.Status( $"Crawling '{settings.Url}'" );

                    await Task.Delay( 1000 );
                    await crawl.ConfigureAwait( false );

                    ValueTask onError( CrawlErrorEvent e, CancellationToken cancellation )
                    {
                        if( e.Url is not null )
                        {
                            AnsiConsole.MarkupLine( $"[red]![/] {e.Url}" );
                        }

                        AnsiConsole.WriteException( e.Exception );
                        return default;
                    }

                    ValueTask onUrlStarted( CrawlUrlStartedEvent e, CancellationToken cancellation )
                    {
                        context.Status( $"Crawling '{e.Url}'" );
                        return default;
                    }

                    ValueTask onUrlResult( CrawlUrlResultEvent e, CancellationToken cancellation )
                    {
                        AnsiConsole.MarkupLine( $"[green]✔[/] {e.Result.Url}" );
                        Interlocked.Increment( ref count );

                        return default;
                    }
                }
           );

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine( $"[green]Completed crawl of {count} urls.[/]" );

        return 0;
    }
}

public sealed class DefaultCommandSettings : CommandSettings
{
    /// <seealso cref="CrawlerOptions.BatchDelay"/>
    [CommandOption( "--batch-delay" )]
    public TimeSpan? BatchDelay { get; init; }

    /// <seealso cref="CrawlerOptions.BatchSize"/>
    [CommandOption( "--batch-size" )]
    public int? BatchSize { get; init; }

    /// <seealso cref="CrawlerOptions.MaxDegreeOfParallelism"/>
    [CommandOption( "--max-parallelism" )]
    public int? MaxParallelism { get; init; }

    [Description( "The url with which to initiate a crawl." )]
    [CommandArgument( 0, "<url>" )]
    public Uri Url { get; init; } = default!;
}