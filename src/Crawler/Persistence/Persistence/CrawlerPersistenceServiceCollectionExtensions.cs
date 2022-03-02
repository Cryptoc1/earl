using Earl.Crawler.Persistence.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Earl.Crawler.Persistence;

/// <summary> Extensions to <see cref="IServiceCollection"/>. </summary>
public static class CrawlerPersistenceServiceCollectionExtensions
{
    /// <summary> Add dependencies that support <see cref="ICrawlerPersistence"/>. </summary>
    /// <param name="services"> The <see cref="IServiceCollection"/> to add dependencies to. </param>
    public static IServiceCollection AddEarlPersistence( this IServiceCollection services )
    {
        ArgumentNullException.ThrowIfNull( services );

        services.TryAddTransient<ICrawlerPersistenceFactory, CrawlerPersistenceFactory>();
        services.TryAddTransient<ICrawlerPersistenceInvoker, CrawlerPersistenceInvoker>();

        return services;
    }
}