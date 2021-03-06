using System.Collections.ObjectModel;
using Earl.Crawler.Middleware.Abstractions;
using Earl.Crawler.Middleware.Http.Abstractions;
using Microsoft.Extensions.Primitives;

namespace Earl.Crawler.Middleware.Http;

/// <summary> Supports the <see cref="IHttpResponseFeature"/> by making a HTTP GET request to the current url using <see cref="IEarlHttpClient"/>. </summary>
public sealed class HttpResponseMiddleware : ICrawlerMiddleware
{
    private readonly IEarlHttpClient client;

    public HttpResponseMiddleware( IEarlHttpClient client )
        => this.client = client;

    /// <inheritdoc/>
    public async Task InvokeAsync( CrawlUrlContext context, CrawlUrlDelegate next )
    {
        ArgumentNullException.ThrowIfNull( context );
        ArgumentNullException.ThrowIfNull( next );

        using var response = await client.GetAsync( context.Url, context.CrawlContext.CrawlCancelled );
        context.Features.Set<IHttpResponseFeature?>( new HttpResponseFeature( response ) );

        var metadata = new HttpResponseMetadata(
            new ReadOnlyDictionary<string, StringValues>(
                response.Content.Headers.ToDictionary(
                    header => header.Key,
                    header => new StringValues( header.Value.ToArray() )
                )
            ),
            response.TotalDuration,
            new ReadOnlyDictionary<string, StringValues>(
                response.Headers.ToDictionary(
                    header => header.Key,
                    header => new StringValues( header.Value.ToArray() )
                )
            ),
            response.ReasonPhrase,
            ( int )response.StatusCode
        );

        context.Result.Metadata.Add( metadata );
        await next( context );
    }
}