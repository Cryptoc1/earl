using Earl.Crawler.Middleware.UrlScraping.Abstractions;
using Earl.Crawler.Middleware.UrlScraping.Abstractions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Middleware.UrlScraping;

/// <summary> Default implementation of <see cref="IUrlScraperFactory"/>. </summary>
public class UrlScraperFactory : IUrlScraperFactory
{
    private static readonly Type UrlScraperFactoryOfTType = typeof( UrlScraperFactory<> );

    private readonly IDictionary<Type, Type> factoryTypeMap;
    private readonly IServiceProvider serviceProvider;

    public UrlScraperFactory( IServiceProvider serviceProvider )
    {
        factoryTypeMap = new Dictionary<Type, Type>();
        this.serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public IUrlScraper Create( IUrlScraperDescriptor descriptor )
    {
        ArgumentNullException.ThrowIfNull( descriptor );

        var factoryType = GetFactoryType( descriptor );
        var factory = ( IUrlScraperFactory )serviceProvider.GetRequiredService( factoryType );
        return factory.Create( descriptor );
    }

    private Type GetFactoryType( IUrlScraperDescriptor descriptor )
    {
        var descriptorType = descriptor.GetType();
        if( !factoryTypeMap.TryGetValue( descriptorType, out var factoryType ) )
        {
            factoryType = UrlScraperFactoryOfTType.MakeGenericType( descriptorType );
            factoryTypeMap.Add( descriptorType, factoryType );
        }

        return factoryType;
    }
}

/// <summary> Describe an <see cref="IUrlScraperFactory"/> for an <see cref="IUrlScraperDescriptor"/> of type <typeparamref name="TDescriptor"/>. </summary>
/// <typeparam name="TDescriptor"> The type of <see cref="IUrlScraperDescriptor"/> handled by the implementing factory. </typeparam>
public abstract class UrlScraperFactory<TDescriptor> : IUrlScraperFactory
    where TDescriptor : IUrlScraperDescriptor
{
    /// <summary> Create an <see cref="IUrlScraper"/> for the given <paramref name="descriptor"/>. </summary>
    /// <param name="descriptor"> The descriptor to be used to create an instance of an <see cref="IUrlScraper"/>. </param>
    public abstract IUrlScraper Create( TDescriptor descriptor );

    /// <inheritdoc/>
    IUrlScraper IUrlScraperFactory.Create( IUrlScraperDescriptor descriptor )
    {
        ArgumentNullException.ThrowIfNull( descriptor );
        return Create( ( TDescriptor )descriptor );
    }
}