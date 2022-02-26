using Earl.Crawler.Abstractions;
using Earl.Crawler.Middleware.Abstractions;

namespace Earl.Crawler.Middleware;

/// <summary> Represents an <see cref="ICrawlerMiddlewareDefinition"/> for a delegate method. </summary>
public class DelegateCrawlerMiddlewareDefinition : ICrawlerMiddlewareDefinition
{
    #region Properties

    /// <summary> The delegate method representing the <see cref="ICrawlerMiddleware.InvokeAsync(CrawlUrlContext, CrawlUrlDelegate)"/> body. </summary>
    public Func<CrawlUrlContext, CrawlUrlDelegate, Task> Middleware;
    #endregion

    public DelegateCrawlerMiddlewareDefinition( Func<CrawlUrlContext, CrawlUrlDelegate, Task> middleware )
        => Middleware = middleware;
}

public class DelegateCrawlerMiddlewareFactory : CrawlerMiddlewareFactory<DelegateCrawlerMiddlewareDefinition>
{
    public override ICrawlerMiddleware Create( DelegateCrawlerMiddlewareDefinition definition )
        => new DelegateCrawlerMiddleware( definition.Middleware );
}

public class DelegateCrawlerMiddleware : ICrawlerMiddleware
{
    #region Fields
    private readonly Func<CrawlUrlContext, CrawlUrlDelegate, Task> middleware;
    #endregion

    public DelegateCrawlerMiddleware( Func<CrawlUrlContext, CrawlUrlDelegate, Task> middleware )
        => this.middleware = middleware;

    public async Task InvokeAsync( CrawlUrlContext context, CrawlUrlDelegate next )
    {
        ArgumentNullException.ThrowIfNull( context );
        ArgumentNullException.ThrowIfNull( next );

        await middleware( context, next );
    }
}
