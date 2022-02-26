using Earl.Crawler.Abstractions;
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
    public ICrawlerOptions Build( )
        => new DefaultCrawlerOptions();

    /// <summary> Creates an instance of <see cref="CrawlerOptionsBuilder"/> with the default crawl configuration. </summary>
    public static ICrawlerOptionsBuilder CreateDefault( )
        => new CrawlerOptionsBuilder()
            .UseMiddleware<HttpResponseMiddleware>()
            .UseMiddleware<HtmlDocumentMiddleware>()
            .UseMiddleware<UrlScraperMiddleware>();

    /// <summary> Decorates the given <paramref name="builder"/> with the given <paramref name="build"/> action. </summary>
    /// <param name="builder"> The builder to be decorated. </param>
    /// <param name="build"> The action to decorate the given <paramref name="builder"/> with. </param>
    public static ICrawlerOptionsBuilder Decorate( ICrawlerOptionsBuilder builder, Action<ICrawlerOptionsBuilder, ICrawlerOptions> build )
    {
        ArgumentNullException.ThrowIfNull( builder );
        ArgumentNullException.ThrowIfNull( build );

        return !IsDecorated( builder, build )
            ? new BuildActionDecorator( builder, build )
            : builder;
    }

    /// <summary> Determines whether the given <paramref name="builder"/> is decorated with the given <paramref name="build"/> action. </summary>
    /// <param name="builder"> The builder to evaluate. </param>
    /// <param name="build"> The build action to evaluate. </param>
    public static bool IsDecorated( ICrawlerOptionsBuilder builder, Action<ICrawlerOptionsBuilder, ICrawlerOptions> build )
    {
        if( builder is BuildActionDecorator decorator )
        {
            if( decorator.BuildAction == build )
            {
                return true;
            }

            if( decorator.Builder is not null )
            {
                return IsDecorated( decorator.Builder, build );
            }
        }

        return false;
    }

    /// <summary> An implementation of <see cref="ICrawlerOptionsBuilder"/> that decorates a given builder with a given build action. </summary>
    public class BuildActionDecorator : ICrawlerOptionsBuilder
    {
        #region Properties

        /// <summary> The builder being decorated. </summary>
        public ICrawlerOptionsBuilder Builder { get; }

        /// <summary> The build action to decorate <see cref="Builder"/> with. </summary>
        public Action<ICrawlerOptionsBuilder, ICrawlerOptions> BuildAction { get; }

        /// <inheritdoc/>
        public IDictionary<object, object?> Properties => Builder.Properties;
        #endregion

        public BuildActionDecorator( ICrawlerOptionsBuilder builder, Action<ICrawlerOptionsBuilder, ICrawlerOptions> build )
        {
            ArgumentNullException.ThrowIfNull( builder );
            ArgumentNullException.ThrowIfNull( build );

            Builder = builder;
            BuildAction = build;
        }

        /// <inheritdoc/>
        public ICrawlerOptions Build( )
        {
            var options = Builder.Build();
            BuildAction( Builder, options );

            return options;
        }
    }
}