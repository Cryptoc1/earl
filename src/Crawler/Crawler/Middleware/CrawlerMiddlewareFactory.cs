using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Middleware.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Middleware;

/// <summary> Default implementation of <see cref="ICrawlerMiddlewareFactory"/>. </summary>
public class CrawlerMiddlewareFactory : ICrawlerMiddlewareFactory
{
    #region Fields
    private static readonly Type CrawlerMiddlewareFactoryOfTType = typeof( CrawlerMiddlewareFactory<> );

    private readonly IDictionary<Type, Type> factoryTypeMap;
    private readonly IServiceProvider serviceProvider;
    #endregion

    public CrawlerMiddlewareFactory( IServiceProvider serviceProvider )
    {
        factoryTypeMap = new Dictionary<Type, Type>();
        this.serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public ICrawlerMiddleware Create( ICrawlerMiddlewareDescriptor descriptor )
    {
        ArgumentNullException.ThrowIfNull( descriptor );

        var factoryType = GetFactoryType( descriptor );
        var factory = ( ICrawlerMiddlewareFactory )serviceProvider.GetRequiredService( factoryType );
        return factory.Create( descriptor );
    }

    private Type GetFactoryType( ICrawlerMiddlewareDescriptor descriptor )
    {
        var descriptorType = descriptor.GetType();
        if( !factoryTypeMap.TryGetValue( descriptorType, out var factoryType ) )
        {
            factoryType = CrawlerMiddlewareFactoryOfTType.MakeGenericType( descriptorType );
            factoryTypeMap.Add( descriptorType, factoryType );
        }

        return factoryType;
    }
}

/// <summary> A base implementation of an <see cref="ICrawlerMiddlewareFactory{TDescriptor}"/>. </summary>
/// <typeparam name="TDescriptor"> The type of <see cref="ICrawlerMiddlewareDescriptor"/> handled by the implementing factory. </typeparam>
public abstract class CrawlerMiddlewareFactory<TDescriptor> : ICrawlerMiddlewareFactory
    where TDescriptor : ICrawlerMiddlewareDescriptor
{
    /// <inheritdoc/>
    public abstract ICrawlerMiddleware Create( TDescriptor descriptor );

    /// <inheritdoc/>
    ICrawlerMiddleware ICrawlerMiddlewareFactory.Create( ICrawlerMiddlewareDescriptor descriptor )
    {
        ArgumentNullException.ThrowIfNull( descriptor );
        return Create( ( TDescriptor )descriptor );
    }
}