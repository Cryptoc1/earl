using Earl.Crawler.Middleware.UrlScraping.Abstractions;
using Earl.Crawler.Middleware.UrlScraping.Abstractions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Middleware.UrlScraping;

/// <summary> Default implementation of <see cref="IUrlFilterFactory"/>. </summary>
public class UrlFilterFactory : IUrlFilterFactory
{
    private static readonly Type UrlFilterFactoryOfTType = typeof( UrlFilterFactory<> );

    private readonly IDictionary<Type, Type> factoryTypeMap;
    private readonly IServiceProvider serviceProvider;

    public UrlFilterFactory( IServiceProvider serviceProvider )
    {
        factoryTypeMap = new Dictionary<Type, Type>();
        this.serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public IUrlFilter Create( IUrlFilterDescriptor descriptor )
    {
        ArgumentNullException.ThrowIfNull( descriptor );

        var factoryType = GetFactoryType( descriptor );
        var factory = ( IUrlFilterFactory )serviceProvider.GetRequiredService( factoryType );
        return factory.Create( descriptor );
    }

    private Type GetFactoryType( IUrlFilterDescriptor descriptor )
    {
        var descriptorType = descriptor.GetType();
        if( !factoryTypeMap.TryGetValue( descriptorType, out var factoryType ) )
        {
            factoryType = UrlFilterFactoryOfTType.MakeGenericType( descriptorType );
            factoryTypeMap.Add( descriptorType, factoryType );
        }

        return factoryType;
    }
}

/// <summary> Describe an <see cref="IUrlFilterFactory"/> for an <see cref="IUrlFilterDescriptor"/> of type <typeparamref name="TDescriptor"/>. </summary>
/// <typeparam name="TDescriptor"> The type of <see cref="IUrlFilterDescriptor"/> handled by the implementing factory. </typeparam>
public abstract class UrlFilterFactory<TDescriptor> : IUrlFilterFactory
    where TDescriptor : IUrlFilterDescriptor
{
    /// <summary> Create an <see cref="IUrlFilter"/> for the given <paramref name="descriptor"/>. </summary>
    /// <param name="descriptor"> The descriptor to be used to create an instance of an <see cref="IUrlFilter"/>. </param>
    public abstract IUrlFilter Create( TDescriptor descriptor );

    /// <inheritdoc/>
    IUrlFilter IUrlFilterFactory.Create( IUrlFilterDescriptor descriptor )
    {
        ArgumentNullException.ThrowIfNull( descriptor );
        return Create( ( TDescriptor )descriptor );
    }
}