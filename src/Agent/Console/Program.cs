using System.Reflection;
using Earl.Crawler;
using Earl.Crawler.Abstractions;
using Earl.Crawler.Configurations;
using Earl.Crawler.Templating;
using Earl.Crawler.Templating.DefaultTemplate;
using Earl.Crawler.Templating.Razor;
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

        private static void ConfigureServices( IServiceCollection services )
        {
            services.AddMemoryCache();
            services.AddOptions();
            services.AddSingleton( PhysicalConsole.Singleton );

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

        private async Task OnExecuteAsync( IEarlCrawler crawler )
        {
            var options = new TemplateCrawlHandlerOptions
            {
                OutputDirectory = @"C:\Users\cryptoc1\Desktop\crawler-results",
            };

            var razor = new RazorLightEngineBuilder()
                .UseProject( new EarlTemplateRazorProject<DefaultTemplateIdentifier>() )
                .UseMemoryCachingProvider()
                .Build();

            var controller = new DefaultTemplateController();
            var executor = new ViewTemplateResultExecutor( razor );
            var namePolicy = new DefaultTemplateNamePolicy();

            var handler = new TemplateCrawlHandler(
                controller,
                executor,
                namePolicy,
                Options.Create( options )
            );

            await crawler.CrawlAsync(
                new Uri( "https://webscraper.io/test-sites/e-commerce/static" ),
                handler,
                new AggressiveCrawlOptions { MaxRequestCount = 1000 }
            );
        }

    }

}
