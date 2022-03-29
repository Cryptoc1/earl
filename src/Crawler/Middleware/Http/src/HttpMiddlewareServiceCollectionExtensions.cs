using System.Net.Http.Headers;
using Earl.Crawler.Middleware.Http.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Middleware.Http;

/// <summary> Extensions to <see cref="IServiceCollection"/>. </summary>
public static class HttpMiddlewareServiceCollectionExtensions
{
    /// <summary> Adds dependencies for the <see cref="HttpResponseMiddleware"/>. </summary>
    /// <param name="services"> The <see cref="IServiceCollection"/> to add dependencies to. </param>
    public static IServiceCollection AddEarlHttpResponse( this IServiceCollection services )
    {
        ArgumentNullException.ThrowIfNull( services );

        services.AddTransient<EarlHttpMessageHandler>();
        services.AddHttpClient<IEarlHttpClient, EarlHttpClient>()
            .AddHttpMessageHandler<EarlHttpMessageHandler>()
            .SetHandlerLifetime( TimeSpan.FromMinutes( 5 ) );

        return services;
    }
}