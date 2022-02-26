using Earl.Agent.Console;
using Earl.Crawler;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

var services = new ServiceCollection()
    .AddLogging()
    .AddMemoryCache()
    .AddOptions()
    .AddEarlCrawler();

var registrar = new TypeRegistrar( services );
var app = new CommandApp<DefaultCommand>( registrar );

app.Configure(
    config =>
    {
#if DEBUG
        config.PropagateExceptions();
        config.ValidateExamples();
#endif
    }
);

return await app.RunAsync( args )
    .ConfigureAwait( false );

internal class TypeRegistrar : ITypeRegistrar
{
    private readonly IServiceCollection services;

    public TypeRegistrar( IServiceCollection services )
    {
        ArgumentNullException.ThrowIfNull( services );
        this.services = services;
    }

    public ITypeResolver Build( ) => new TypeResolver( services.BuildServiceProvider() );
    public void Register( Type service, Type implementation ) => services.AddSingleton( service, implementation );
    public void RegisterInstance( Type service, object implementation ) => services.AddSingleton( service, implementation );
    public void RegisterLazy( Type service, Func<object> factory )
    {
        ArgumentNullException.ThrowIfNull( factory );
        services.AddSingleton( service, _ => factory() );
    }

    private class TypeResolver : ITypeResolver, IDisposable
    {
        private readonly IServiceProvider serviceProvider;

        public TypeResolver( IServiceProvider serviceProvider )
        {
            ArgumentNullException.ThrowIfNull( serviceProvider );
            this.serviceProvider = serviceProvider;
        }

        public void Dispose( )
        {
            if( serviceProvider is IDisposable disposable )
            {
                disposable.Dispose();
            }
        }

        public object? Resolve( Type? type )
        {
            ArgumentNullException.ThrowIfNull( type );
            return serviceProvider.GetService( type );
        }
    }
}