using Earl.Crawler.Persistence.Abstractions;
using Earl.Crawler.Persistence.Abstractions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Persistence;

/// <summary> Default implementation of <see cref="ICrawlerPersistenceFactory"/>. </summary>
public sealed class CrawlerPersistenceFactory : ICrawlerPersistenceFactory
{
    private static readonly Type CrawlerPersistenceFactoryOfTType = typeof( CrawlerPersistenceFactory<> );

    private readonly IDictionary<Type, Type> factoryTypeMap;
    private readonly IServiceProvider serviceProvider;

    public CrawlerPersistenceFactory( IServiceProvider serviceProvider )
    {
        factoryTypeMap = new Dictionary<Type, Type>();
        this.serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public ICrawlerPersistence Create( ICrawlerPersistenceDescriptor descriptor )
    {
        ArgumentNullException.ThrowIfNull( descriptor );

        var factoryType = GetFactoryType( descriptor );
        var factory = ( ICrawlerPersistenceFactory )serviceProvider.GetRequiredService( factoryType );
        return factory.Create( descriptor );
    }

    private Type GetFactoryType( ICrawlerPersistenceDescriptor descriptor )
    {
        var descriptorType = descriptor.GetType();
        if( !factoryTypeMap.TryGetValue( descriptorType, out var factoryType ) )
        {
            factoryType = CrawlerPersistenceFactoryOfTType.MakeGenericType( descriptorType );
            factoryTypeMap.Add( descriptorType, factoryType );
        }

        return factoryType;
    }
}

/// <summary> Describe an <see cref="ICrawlerPersistenceFactory"/> for an <see cref="ICrawlerPersistenceDescriptor"/> of type <typeparamref name="TDescriptor"/>. </summary>
/// <typeparam name="TDescriptor"> The type of <see cref="ICrawlerPersistenceDescriptor"/> handled by the implementing factory. </typeparam>
public abstract class CrawlerPersistenceFactory<TDescriptor> : ICrawlerPersistenceFactory
    where TDescriptor : ICrawlerPersistenceDescriptor
{
    /// <summary> Create an <see cref="ICrawlerPersistence"/> for the given <paramref name="descriptor"/>. </summary>
    /// <param name="descriptor"> The descriptor to be used to create an instance of an <see cref="ICrawlerPersistence"/>. </param>
    public abstract ICrawlerPersistence Create( TDescriptor descriptor );

    /// <inheritdoc/>
    ICrawlerPersistence ICrawlerPersistenceFactory.Create( ICrawlerPersistenceDescriptor descriptor )
    {
        ArgumentNullException.ThrowIfNull( descriptor );
        return Create( ( TDescriptor )descriptor );
    }
}