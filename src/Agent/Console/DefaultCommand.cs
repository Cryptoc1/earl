﻿using Earl.Agent.Console.Abstractions;
using Earl.Crawler.Abstractions;
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
                        .WithHandler<CrawlErrorEvent>( onCrawlError )
                        .WithHandler<CrawlResultEvent>( onCrawlResult )

                        /* .UseMiddleware(
                            ( context, next ) =>
                            {
                                AnsiConsole.WriteLine( $"Executing middleware while crawling '{context.Url}'." );
                                return next( context );
                            }
                        ) */

                        .Build();

                    var crawl = crawler.CrawlAsync( url, options, cancellation );

                    context.SpinnerStyle( Style.Parse( "green bold" ) );
                    context.Spinner( Spinner.Known.Triangle );
                    context.Status( $"Crawling" );

                    await Task.Delay( 1500 );
                    await crawl;

                    static Task onCrawlError( Uri url, Exception exception, CancellationToken cancellation )
                    {
                        AnsiConsole.MarkupLine( $"[red]![/] {url}" );
                        return Task.CompletedTask;
                    }

                    static Task onCrawlResult( CrawlUrlResult result, CancellationToken cancellation )
                    {
                        AnsiConsole.MarkupLine( $"[green]✔[/] {result.Url}" );
                        return Task.CompletedTask;
                    }
                }
            );

        return 0;
    }
}