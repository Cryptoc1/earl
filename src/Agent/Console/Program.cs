using System.Reflection;
using Earl.Crawler;
using Earl.Crawler.Abstractions;
using Earl.Crawler.Configurations;
using Earl.Crawler.Reporting.Templating;
using Earl.Crawler.Templating.DefaultTemplate;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RazorLight;

namespace Earl.Agent
{

    [Command( "earl", Description = "Earl Agent: executes request profiles." )]
    [HelpOption]
    [VersionOptionFromMember( "--version", MemberName = nameof( GetVersion ) )]
    public class Program
    {

        public static string? GetVersion( )
            => typeof( Program ).Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion;

        public static Task<int> Main( string[] args )
            => Host.CreateDefaultBuilder( args )
                .UseEnvironment( Environments.Development )
                .ConfigureServices(
                    ( context, services ) => services.AddSingleton( PhysicalConsole.Singleton )
                        .AddOptions()
                        .AddMemoryCache()
                        .AddEarlCrawler()
                )

                .RunCommandLineApplicationAsync<Program>( args );

        private async Task OnExecuteAsync( IEarlCrawler crawler )
        {
            var url = new Uri( "https://webscraper.io/test-sites/e-commerce/static" );
            var options = new AggressiveCrawlOptions { MaxRequestCount = 1000 };

            /* var results = new ConcurrentBag<CrawlUrlResult>();
            var reporter = new DelegateCrawlReporter(
                result =>
                {
                    results.Add( result );
                    return Task.CompletedTask;
                }
            ); */

            var razor = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject( typeof( DefaultTemplateIdentifier ) )
                .UseMemoryCachingProvider()
                .Build();

            var templateOptions = new TemplateCrawlHandlerOptions
            {
                OutputDirectory = @"C:\Users\cryptoc1\Desktop\crawler-results"
            };

            var handler = new TemplateCrawlHandler( Options.Create( templateOptions ), razor );
            await crawler.CrawlAsync( url, handler, options );
        }

    }

}
