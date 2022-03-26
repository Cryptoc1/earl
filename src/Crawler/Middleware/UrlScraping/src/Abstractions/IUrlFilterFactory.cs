using Earl.Crawler.Middleware.UrlScraping.Abstractions.Configuration;

namespace Earl.Crawler.Middleware.UrlScraping.Abstractions;

/// <summary> Describes a service that can create an instance of an <see cref="IUrlFilter"/> for a given <see cref="IUrlFilterDescriptor"/>. </summary>
public interface IUrlFilterFactory
{
    /// <summary> Create an <see cref="IUrlFilter"/> for the given <paramref name="descriptor"/>. </summary>
    /// <param name="descriptor"> The descriptor to be used to create an instance of an <see cref="IUrlFilter"/>. </param>
    IUrlFilter Create( IUrlFilterDescriptor descriptor );
}