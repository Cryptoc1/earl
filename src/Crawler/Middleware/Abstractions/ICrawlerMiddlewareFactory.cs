using Earl.Crawler.Abstractions.Configuration;

namespace Earl.Crawler.Middleware.Abstractions;

/// <summary> Describes a service that can create an instance of an <see cref="ICrawlerMiddleware"/> for a given <see cref="ICrawlerMiddlewareDescriptor"/>. </summary>
public interface ICrawlerMiddlewareFactory
{
    /// <summary> Create an <see cref="ICrawlerMiddleware"/> for the given <paramref name="descriptor"/>. </summary>
    /// <param name="descriptor"> The descriptor to be used to create an instance of an <see cref="ICrawlerMiddleware"/>. </param>
    ICrawlerMiddleware Create( ICrawlerMiddlewareDescriptor descriptor );
}