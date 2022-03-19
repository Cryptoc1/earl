using System.Net.Http.Headers;
using Earl.Crawler.Middleware.Http.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Middleware.Http;

/// <summary> Extensions to <see cref="IServiceCollection"/>. </summary>
public static class HttpMiddlewareServiceCollectionExtensions
{
    private static readonly Version ClientVersion = typeof( EarlHttpClient ).Assembly.GetName().Version!;

    /// <summary> Adds dependencies for the <see cref="HttpResponseMiddleware"/>. </summary>
    /// <param name="services"> The <see cref="IServiceCollection"/> to add dependencies to. </param>
    public static IServiceCollection AddEarlHttpResponse( this IServiceCollection services )
    {
        ArgumentNullException.ThrowIfNull( services );

        services.AddTransient<EarlHttpMessageHandler>();
        services.AddHttpClient<IEarlHttpClient, EarlHttpClient>()
            .ConfigureHttpClient(
                client =>
                {
                    var productValue = new ProductInfoHeaderValue( nameof( EarlHttpClient ), ClientVersion!.ToString() );
                    var commentValue = new ProductInfoHeaderValue( "(+https://www.github.com/cryptoc1/earl)" );

                    client.DefaultRequestHeaders.UserAgent.Add( productValue );
                    client.DefaultRequestHeaders.UserAgent.Add( commentValue );
                }
            )
            .AddHttpMessageHandler<EarlHttpMessageHandler>()
            .SetHandlerLifetime( TimeSpan.FromMinutes( 5 ) );

        return services;
    }
}