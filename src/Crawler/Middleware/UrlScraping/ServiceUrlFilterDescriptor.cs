using Earl.Crawler.Middleware.UrlScraping.Abstractions;
using Earl.Crawler.Middleware.UrlScraping.Abstractions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Middleware.UrlScraping;

/// <summary> Represents an <see cref="IUrlFilterDescriptor"/> for a <see cref="Type"/> that can be activated via <see cref="ActivatorUtilities"/>. </summary>
public sealed class ServiceUrlFilterDescriptor : IUrlFilterDescriptor
{
    /// <summary> The <see cref="Type"/> of <see cref="IUrlFilter"/> being defined. </summary>
    public Type FilterType { get; }

    private static readonly Type IUrlFilterType = typeof( IUrlFilter );

    public ServiceUrlFilterDescriptor( Type type )
    {
        if( !type.IsAssignableTo( IUrlFilterType ) )
        {
            throw new ArgumentException( $"Given type '{type.Name}' does not implement '{nameof( IUrlFilter )}'.", nameof( type ) );
        }

        FilterType = type;
    }
}

/// <summary> An <see cref="IUrlFilterFactory"/> for handling the <see cref="ServiceUrlFilterDescriptor"/>. </summary>
public class ServiceUrlFilterFactory : UrlFilterFactory<ServiceUrlFilterDescriptor>
{
    private readonly IServiceProvider serviceProvider;

    public ServiceUrlFilterFactory( IServiceProvider serviceProvider )
        => this.serviceProvider = serviceProvider;

    public override IUrlFilter Create( ServiceUrlFilterDescriptor descriptor )
        => ( IUrlFilter )ActivatorUtilities.CreateInstance( serviceProvider, descriptor.FilterType );
}