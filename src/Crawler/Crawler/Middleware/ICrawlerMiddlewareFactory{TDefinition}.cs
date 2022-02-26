using Earl.Crawler.Abstractions;
using Earl.Crawler.Middleware.Abstractions;

namespace Earl.Crawler.Middleware;

/// <summary> Describes a service that can create an instance of an <see cref="ICrawlerMiddleware"/> for the specified <see cref="ICrawlerMiddlewareDefinition"/>. </summary>
public interface ICrawlerMiddlewareFactory
{
    ICrawlerMiddleware Create( ICrawlerMiddlewareDefinition definition );
}

/// <typeparam name="TDefinition"> The type of <see cref="ICrawlerMiddlewareDefinition"/> handled by the factory. </typeparam>
public interface ICrawlerMiddlewareFactory<TDefinition> : ICrawlerMiddlewareFactory
    where TDefinition : ICrawlerMiddlewareDefinition
{
    ICrawlerMiddleware Create( TDefinition definition );
}

public abstract class CrawlerMiddlewareFactory<TDefinition> : ICrawlerMiddlewareFactory<TDefinition>
    where TDefinition : ICrawlerMiddlewareDefinition
{
    public abstract ICrawlerMiddleware Create( TDefinition definition );
    ICrawlerMiddleware ICrawlerMiddlewareFactory.Create( ICrawlerMiddlewareDefinition definition )
        => Create( ( TDefinition )definition );
}