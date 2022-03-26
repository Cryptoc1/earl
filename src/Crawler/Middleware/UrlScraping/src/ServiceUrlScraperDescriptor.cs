using Earl.Crawler.Middleware.UrlScraping.Abstractions;
using Earl.Crawler.Middleware.UrlScraping.Abstractions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Earl.Crawler.Middleware.UrlScraping;

/// <summary> Represents an <see cref="IUrlScraperDescriptor"/> for a <see cref="Type"/> that can be activated via <see cref="ActivatorUtilities"/>. </summary>
public sealed class ServiceUrlScraperDescriptor : IUrlScraperDescriptor
{
    /// <summary> The <see cref="Type"/> of <see cref="IUrlScraper"/> being defined. </summary>
    public Type ScraperType { get; }

    private static readonly Type IUrlScraperType = typeof( IUrlScraper );

    public ServiceUrlScraperDescriptor( Type type )
    {
        if( !type.IsAssignableTo( IUrlScraperType ) )
        {
            throw new ArgumentException( $"Given type '{type.Name}' does not implement '{nameof( IUrlScraper )}'.", nameof( type ) );
        }

        ScraperType = type;
    }
}

/// <summary> An <see cref="IUrlScraperFactory"/> for handling the <see cref="ServiceUrlScraperDescriptor"/>. </summary>
public class ServiceUrlScraperFactory : UrlScraperFactory<ServiceUrlScraperDescriptor>
{
    private readonly IServiceProvider serviceProvider;

    public ServiceUrlScraperFactory( IServiceProvider serviceProvider )
        => this.serviceProvider = serviceProvider;

    /// <inheritdoc/>
    public override IUrlScraper Create( ServiceUrlScraperDescriptor descriptor )
        => ( IUrlScraper )ActivatorUtilities.CreateInstance( serviceProvider, descriptor.ScraperType );
}