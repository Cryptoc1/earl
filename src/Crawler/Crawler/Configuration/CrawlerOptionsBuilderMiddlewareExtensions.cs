﻿using Earl.Crawler.Abstractions.Configuration;
using Earl.Crawler.Middleware;
using Earl.Crawler.Middleware.Abstractions;

namespace Earl.Crawler.Configuration;

/// <summary> Extensions to <see cref="ICrawlerOptionsBuilder"/> for registering <see cref="ICrawlerMiddlewareDescriptor"/>s. </summary>
public static class CrawlerOptionsBuilderMiddlewareExtensions
{
    private static CrawlerOptions BuildMiddleware( ICrawlerOptionsBuilder builder, CrawlerOptions options )
    {
        ArgumentNullException.ThrowIfNull( builder );
        ArgumentNullException.ThrowIfNull( options );

        foreach( var descriptor in MiddlewareDescriptorsProperty( builder ) )
        {
            options.Middleware.Add( descriptor );
        }

        return options;
    }

    private static IList<ICrawlerMiddlewareDescriptor> MiddlewareDescriptorsProperty( ICrawlerOptionsBuilder builder )
        => builder.GetOrAddProperty( nameof( CrawlerOptions.Middleware ), ( ) => new List<ICrawlerMiddlewareDescriptor>() );

    /// <summary> Register the middleware of type <typeparamref name="TMiddleware"/>. </summary>
    /// <typeparam name="TMiddleware"> The type of middleware to be registered. </typeparam>
    /// <param name="builder"> The options builder to register the handler with. </param>
    public static ICrawlerOptionsBuilder Use<TMiddleware>( this ICrawlerOptionsBuilder builder )
        where TMiddleware : ICrawlerMiddleware
        => Use( builder, new ServiceCrawlerMiddlewareDescriptor( typeof( TMiddleware ) ) );

    /// <summary> Register the given delegate as middleware. </summary>
    /// <param name="builder"> The options builder to register the handler with. </param>
    /// <param name="middleware"> The delegate to register as middleware. </param>
    public static ICrawlerOptionsBuilder Use( this ICrawlerOptionsBuilder builder, Func<CrawlUrlContext, CrawlUrlDelegate, Task> middleware )
    {
        ArgumentNullException.ThrowIfNull( middleware );
        return Use( builder, new DelegateCrawlerMiddlewareDescriptor( middleware ) );
    }

    /// <summary> Register the middleware descriptor. </summary>
    /// <param name="builder"> The options builder to register the handler with. </param>
    /// <param name="descriptor"> The <see cref="ICrawlerMiddlewareDescriptor"/> to register. </param>
    public static ICrawlerOptionsBuilder Use( this ICrawlerOptionsBuilder builder, ICrawlerMiddlewareDescriptor descriptor )
    {
        ArgumentNullException.ThrowIfNull( builder );
        ArgumentNullException.ThrowIfNull( descriptor );

        MiddlewareDescriptorsProperty( builder )
            .Add( descriptor );

        return CrawlerOptionsBuilder.Decorate( builder, BuildMiddleware );
    }
}