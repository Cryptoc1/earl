﻿using System.Reflection;
using Earl.Crawler;
using Earl.Crawler.Abstractions;
using Earl.Crawler.Configurations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            /* if( UseSelenium )
            {
                options.UseSelenium();
            } */

            var result = await crawler.CrawlAsync( url, options );

            return;
        }

    }

}
