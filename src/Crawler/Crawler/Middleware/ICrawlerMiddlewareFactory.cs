using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Middleware.Abstractions;

namespace Earl.Crawler.Middleware;

/// <summary> Describes a service that can create an instance of an <see cref="ICrawlerMiddleware"/> for a given <see cref="ICrawlerMiddlewareDescriptor"/>. </summary>
public interface ICrawlerMiddlewareFactory
{
    /// <summary> Create an <see cref="ICrawlerMiddleware"/> for the given <paramref name="descriptor"/>. </summary>
    /// <param name="descriptor"> The descriptor to be used to create an instance of an <see cref="ICrawlerMiddleware"/>. </param>
    ICrawlerMiddleware Create( ICrawlerMiddlewareDescriptor descriptor );
}

/// <summary> Describes a service that can create an instance of an <see cref="ICrawlerMiddleware"/> for a given <see cref="TDescriptor"/>. </summary>
/// <typeparam name="TDescriptor"> The type of <see cref="ICrawlerMiddlewareDescriptor"/> handled by the factory. </typeparam>
public interface ICrawlerMiddlewareFactory<TDescriptor> : ICrawlerMiddlewareFactory
    where TDescriptor : ICrawlerMiddlewareDescriptor
{
    /// <summary> Create an <see cref="ICrawlerMiddleware"/> for the given <paramref name="descriptor"/>. </summary>
    /// <param name="descriptor"> The descriptor to be used to create an instance of an <see cref="ICrawlerMiddleware"/>. </param>
    ICrawlerMiddleware Create( TDescriptor descriptor );
}

/// <summary> A base implementation of an <see cref="ICrawlerMiddlewareFactory{TDescriptor}"/>. </summary>
/// <typeparam name="TDescriptor"> The type of <see cref="ICrawlerMiddlewareDescriptor"/> handled by the implementing factory. </typeparam>
public abstract class CrawlerMiddlewareFactory<TDescriptor> : ICrawlerMiddlewareFactory<TDescriptor>
    where TDescriptor : ICrawlerMiddlewareDescriptor
{
    /// <inheritdoc/>
    public abstract ICrawlerMiddleware Create( TDescriptor descriptor );

    /// <inheritdoc/>
    ICrawlerMiddleware ICrawlerMiddlewareFactory.Create( ICrawlerMiddlewareDescriptor descriptor )
        => Create( ( TDescriptor )descriptor );
}