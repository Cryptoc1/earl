using Earl.Crawler.Abstractions;
using Earl.Crawler.Middleware.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Middleware;

/// <summary> Represents an <see cref="ICrawlerMiddlewareDefinition"/> for a <see cref="Type"/> that can be activated via <see cref="ActivatorUtilities.GetServiceOrCreateInstance(IServiceProvider, Type)"/>. </summary>
public class ServiceCrawlerMiddlewareDefinition : ICrawlerMiddlewareDefinition
{
    #region Fields
    private static readonly Type ICrawlerMiddlewareType = typeof( ICrawlerMiddleware );
    #endregion

    #region Properties

    /// <summary> The <see cref="Type"/> of <see cref="ICrawlerMiddleware"/> being defined. </summary>
    public Type MiddlewareType { get; }
    #endregion

    public ServiceCrawlerMiddlewareDefinition( Type type )
    {
        ArgumentNullException.ThrowIfNull( type );
        if( !type.IsAssignableTo( ICrawlerMiddlewareType ) )
        {
            throw new ArgumentException( $"Given type '{type.Name}' does not implement '{nameof( ICrawlerMiddleware )}'.", nameof( type ) );
        }

        MiddlewareType = type;
    }
}

public class ServiceCrawlerMiddlewareFactory : CrawlerMiddlewareFactory<ServiceCrawlerMiddlewareDefinition>
{
    #region Fields
    private readonly IServiceProvider serviceProvider;
    #endregion

    public ServiceCrawlerMiddlewareFactory( IServiceProvider serviceProvider )
        => this.serviceProvider = serviceProvider;

    public override ICrawlerMiddleware Create( ServiceCrawlerMiddlewareDefinition definition )
        => ( ICrawlerMiddleware )ActivatorUtilities.GetServiceOrCreateInstance( serviceProvider, definition.MiddlewareType );
}