using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Events;
using Earl.Crawler.Middleware.Html;
using Earl.Crawler.Middleware.Http;
using Earl.Crawler.Middleware.UrlScraping;

namespace Earl.Crawler.Configuration;

/// <summary> Default implementation of <see cref="ICrawlerOptionsBuilder"/>. </summary>
public class CrawlerOptionsBuilder : ICrawlerOptionsBuilder
{
    #region Properties

    /// <inheritdoc/>
    public IDictionary<object, object?> Properties { get; } = new Dictionary<object, object?>();
    #endregion

    /// <inheritdoc/>
    public CrawlerOptions Build( )
        => new(
            BatchDelay: null,
            BatchSize: Environment.ProcessorCount * 4,
            Events: new CrawlEvents(),
            MaxDegreeOfParallelism: Environment.ProcessorCount,
            MaxRequestCount: -1,
            Middleware: new List<ICrawlerMiddlewareDescriptor>(),
            Timeout: null
        );

    /// <summary> Creates an instance of <see cref="CrawlerOptionsBuilder"/> with the default crawl configuration. </summary>
    public static ICrawlerOptionsBuilder CreateDefault( )
        => new CrawlerOptionsBuilder()
            .Use<HttpResponseMiddleware>()
            .Use<HtmlDocumentMiddleware>()
            .Use<UrlScraperMiddleware>();

    /// <summary> Decorates the given <paramref name="builder"/> with the given <paramref name="decorator"/> action. </summary>
    /// <param name="builder"> The builder to be decorated. </param>
    /// <param name="decorator"> The action to decorate the given <paramref name="builder"/> with. </param>
    public static ICrawlerOptionsBuilder Decorate( ICrawlerOptionsBuilder builder, BuildActionDecorator decorator )
    {
        ArgumentNullException.ThrowIfNull( builder );
        ArgumentNullException.ThrowIfNull( decorator );

        return !IsDecorated( builder, decorator )
            ? new DecoratedCrawlerOptionsBuilder( builder, decorator )
            : builder;
    }

    /// <summary> Determines whether the given <paramref name="builder"/> is decorated with the given <paramref name="decorator"/> action. </summary>
    /// <param name="builder"> The builder to evaluate. </param>
    /// <param name="decorator"> The build action to evaluate. </param>
    public static bool IsDecorated( ICrawlerOptionsBuilder builder, BuildActionDecorator decorator )
    {
        if( builder is DecoratedCrawlerOptionsBuilder decoratedBuilder )
        {
            if( decoratedBuilder.Decorator == decorator )
            {
                return true;
            }

            if( decoratedBuilder.Builder is not null )
            {
                return IsDecorated( decoratedBuilder.Builder, decorator );
            }
        }

        return false;
    }

    /// <summary> Describes a method that decorates the <see cref="ICrawlerOptionsBuilder.Build"/> method. </summary>
    /// <seealso cref="DecoratedCrawlerOptionsBuilder"/>
    public delegate CrawlerOptions BuildActionDecorator( ICrawlerOptionsBuilder builder, CrawlerOptions options );

    /// <summary> An implementation of <see cref="ICrawlerOptionsBuilder"/> that decorates a given builder with a given <see cref="BuildActionDecorator"/>. </summary>
    public class DecoratedCrawlerOptionsBuilder : ICrawlerOptionsBuilder
    {
        #region Properties

        /// <summary> The builder being decorated. </summary>
        public ICrawlerOptionsBuilder Builder { get; }

        /// <summary> The build action to decorate <see cref="Builder"/> with. </summary>
        public BuildActionDecorator Decorator { get; }

        /// <inheritdoc/>
        public IDictionary<object, object?> Properties => Builder.Properties;
        #endregion

        public DecoratedCrawlerOptionsBuilder( ICrawlerOptionsBuilder builder, BuildActionDecorator decorator )
        {
            ArgumentNullException.ThrowIfNull( builder );
            ArgumentNullException.ThrowIfNull( decorator );

            Builder = builder;
            Decorator = decorator;
        }

        /// <inheritdoc/>
        public CrawlerOptions Build( )
        {
            var options = Builder.Build();
            return Decorator( Builder, options );
        }
    }
}