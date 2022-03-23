using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Middleware.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Middleware;

/// <summary> Represents an <see cref="ICrawlerMiddlewareDescriptor"/> for a <see cref="Type"/> that can be activated via <see cref="ActivatorUtilities"/>. </summary>
public sealed class ServiceCrawlerMiddlewareDescriptor : ICrawlerMiddlewareDescriptor
{
    /// <summary> The <see cref="Type"/> of <see cref="ICrawlerMiddleware"/> being defined. </summary>
    public Type MiddlewareType { get; }

    /// <summary> The middleware options. </summary>
    public object? Options { get; }

    private static readonly Type ICrawlerMiddlewareType = typeof( ICrawlerMiddleware );
    private static readonly Type ICrawlerMiddlewareOfTType = typeof( ICrawlerMiddleware<> );

    public ServiceCrawlerMiddlewareDescriptor( Type type, object? options = null )
    {
        ArgumentNullException.ThrowIfNull( type );
        if( !type.IsAssignableTo( ICrawlerMiddlewareType ) )
        {
            throw new ArgumentException( $"Given type '{type.Name}' does not implement '{nameof( ICrawlerMiddleware )}'.", nameof( type ) );
        }

        if( type.GetInterface( ICrawlerMiddlewareOfTType.Name ) is not null )
        {
            ArgumentNullException.ThrowIfNull( options );
        }

        MiddlewareType = type;
        Options = options;
    }
}

/// <summary> An <see cref="ICrawlerMiddlewareFactory"/> for handling the <see cref="ServiceCrawlerMiddlewareDescriptor"/>. </summary>
public class ServiceCrawlerMiddlewareFactory : CrawlerMiddlewareFactory<ServiceCrawlerMiddlewareDescriptor>
{
    private readonly IServiceProvider serviceProvider;

    public ServiceCrawlerMiddlewareFactory( IServiceProvider serviceProvider )
        => this.serviceProvider = serviceProvider;

    /// <inheritdoc/>
    public override ICrawlerMiddleware Create( ServiceCrawlerMiddlewareDescriptor descriptor )
        => descriptor.Options is not null
            ? ( ICrawlerMiddleware )ActivatorUtilities.CreateInstance( serviceProvider, descriptor.MiddlewareType, descriptor.Options )
            : ( ICrawlerMiddleware )ActivatorUtilities.CreateInstance( serviceProvider, descriptor.MiddlewareType );
}