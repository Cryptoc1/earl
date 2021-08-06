using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Earl.Crawler;
using Earl.Crawler.Abstractions;
using Earl.Crawler.Profiles;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Earl.Agent
{

    [Command( "earl", Description = "Earl Agent: executes request profiles." )]
    [VersionOptionFromMember( "--version", MemberName = nameof( GetVersion ) )]
    [HelpOption]
    public class Program
    {
        #region Fields
        private static readonly Dictionary<string, string> hostConfiguration = new()
        {
            { HostDefaults.ApplicationKey, "Earl Agent" }
        };
        #endregion

        #region Properties
        //[FileExists]
        //[Option( Description = "The path to the file to use for configuration of the agent." )]
        //public string Config { get; set; }
        #endregion

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
            var url = new Uri( "https://vermeer.com.develop/na" );
            var result = await crawler.CrawlAsync( url, new AggressiveCrawlOptions { MaxRequestCount = 1000 } );

            return;
        }

    }

}
