using System.Collections.Concurrent;
using System.Reflection;
using Earl.Crawler;
using Earl.Crawler.Abstractions;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IO;
using WebMarkupMin.Core;
using WebMarkupMin.NUglify;

namespace Earl.Agent.Console;

[Command( "earl", Description = "Earl Agent: executes request profiles." )]
[HelpOption]
[VersionOptionFromMember( "--version", MemberName = nameof( GetVersion ) )]
public class Program
{
    private static void ConfigureServices( IServiceCollection services )
    {
        services.AddMemoryCache();
        services.AddOptions();
        services.AddSingleton( PhysicalConsole.Singleton );

        services.AddTransient<ICssMinifierFactory, NUglifyCssMinifierFactory>();
        services.AddTransient<IJsMinifierFactory, NUglifyJsMinifierFactory>();
        services.AddTransient<IMarkupMinifier>(
            serviceProvider =>
            {
                var cssMinifier = serviceProvider.GetService<ICssMinifierFactory>()
                    ?.CreateMinifier();

                var jsMinifier = serviceProvider.GetService<IJsMinifierFactory>()
                    ?.CreateMinifier();

                return new HtmlMinifier( cssMinifier: cssMinifier, jsMinifier: jsMinifier );
            }
        );

        services.AddSingleton( _ => new RecyclableMemoryStreamManager() );

        //services.AddRazorLight()
        //    .UseEarlTemplateProject<DefaultTemplateIdentifier>()
        //    .UseMemoryCachingProvider();

        services.AddEarlCrawler();
    }

    public static string? GetVersion( )
        => typeof( Program ).Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion;

    public static Task<int> Main( string[] args )
        => Host.CreateDefaultBuilder( args )
            .UseEnvironment( Environments.Development )
            .ConfigureServices( ( context, services ) => ConfigureServices( services ) )
            .RunCommandLineApplicationAsync<Program>( args );

    private async Task OnExecuteAsync(
        //IMarkupMinifier markupMinifier,
        IEarlCrawler crawler
    //RecyclableMemoryStreamManager streamManager
    )
    {
        /* var options = new TemplateCrawlHandlerOptions
        {
            OutputDirectory = @"C:\Users\cryptoc1\Desktop\crawler-results",
        };

        var razor = new RazorLightEngineBuilder()
            .UseProject( new EarlTemplateRazorProject<DefaultTemplateIdentifier>() )
            .UseMemoryCachingProvider()
            .Build();

        var controller = new DefaultTemplateController();
        var executor = new ViewTemplateResultExecutor( markupMinifier, razor, streamManager );
        var namePolicy = new DefaultTemplateNamePolicy();

        var handler = new TemplateCrawlHandler(
            controller,
            executor,
            namePolicy,
            Options.Create( options )
        ); */

        var results = new ConcurrentBag<CrawlUrlResult>();
        var handler = new DelegateCrawlHandler(
            async result =>
            {
                results.Add( result );
                await ValueTask.CompletedTask;
            }
        );

        /* var url = new Uri( "https://webscraper.io/test-sites/e-commerce/static" );
        var options = new CrawlOptions
        {
            MaxRequestCount = 750,
            RequestDelay = TimeSpan.FromSeconds( 2 ),
            Timeout = TimeSpan.FromMinutes( 30 ),
        };

        var crawl = crawler.CrawlAsync( url, options );

        await foreach( var result in crawl )
        {
        } */

        await crawler.CrawlAsync(
            new Uri( "https://webscraper.io/test-sites/e-commerce/static" ),
            handler,
            new CrawlOptions
            {
                MaxRequestCount = 750,
                RequestDelay = TimeSpan.FromSeconds( 2 ),
                Timeout = TimeSpan.FromMinutes( 30 ),
            }
        );

        return;
    }
}