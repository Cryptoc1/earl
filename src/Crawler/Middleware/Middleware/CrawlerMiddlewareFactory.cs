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

/// <summary> Describe an <see cref="ICrawlerMiddlewareFactory"/> for an <see cref="ICrawlerMiddlewareDescriptor"/> of type <typeparamref name="TDescriptor"/>. </summary>
/// <typeparam name="TDescriptor"> The type of <see cref="ICrawlerMiddlewareDescriptor"/> handled by the implementing factory. </typeparam>
public abstract class CrawlerMiddlewareFactory<TDescriptor> : ICrawlerMiddlewareFactory
    where TDescriptor : ICrawlerMiddlewareDescriptor
{
    /// <summary> Create an <see cref="ICrawlerMiddleware"/> for the given <paramref name="descriptor"/>. </summary>
    /// <param name="descriptor"> The descriptor to be used to create an instance of an <see cref="ICrawlerMiddleware"/>. </param>
    public abstract ICrawlerMiddleware Create( TDescriptor descriptor );

    /// <inheritdoc/>
    ICrawlerMiddleware ICrawlerMiddlewareFactory.Create( ICrawlerMiddlewareDescriptor descriptor )
    {
        ArgumentNullException.ThrowIfNull( descriptor );
        return Create( ( TDescriptor )descriptor );
    }
}