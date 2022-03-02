using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Persistence.Json;

/// <summary> Extensions to <see cref="IServiceCollection"/>. </summary>
public static class CrawlerJsonPersistenceServiceCollectionExtensions
{
    /// <summary> Add dependencies that support <see cref="CrawlerJsonPersistence"/>. </summary>
    /// <param name="services"> The <see cref="IServiceCollection"/> to add dependencies to. </param>
    public static IServiceCollection AddCrawlerJsonPersistence( this IServiceCollection services )
    {
        ArgumentNullException.ThrowIfNull( services );

        services.AddTransient<CrawlerPersistenceFactory<CrawlerJsonPersistenceDescriptor>, CrawlerJsonPersistenceFactory>();
        return services;
    }
}