using System.Net.Http.Headers;
using AngleSharp;
using AngleSharp.Html.Dom;
using Earl.Crawler.Middleware.Abstractions;
using Earl.Crawler.Middleware.Html.Abstractions;
using Earl.Crawler.Middleware.Http.Abstractions;
using Microsoft.Extensions.Primitives;

namespace Earl.Crawler.Middleware.Html;

/// <summary> Supports the <see cref="IHtmlDocumentFeature"/>. </summary>
/// <remarks> The implementation of this middleware relies on the <see cref="IHttpResponseFeature.Response"/> to produce an <see cref="IHtmlDocument"/>. </remarks>
public class HtmlDocumentMiddleware : ICrawlerMiddleware
{
    /// <inheritdoc/>
    public async Task InvokeAsync( CrawlUrlContext context, CrawlUrlDelegate next )
    {
        ArgumentNullException.ThrowIfNull( context );
        ArgumentNullException.ThrowIfNull( next );

        var responseFeature = context.Features.Get<IHttpResponseFeature>();

        var document = await GetDocumentAsync( responseFeature!, context.CrawlContext.CrawlCancelled );
        if( document is not null )
        {
            // NOTE: feature is disposed by the context
            context.Features.Set<IHtmlDocumentFeature?>( new HtmlDocumentFeature( document ) );
        }

        context.Result.DisplayName = $"Crawl Results for '{document!.Title}'";
        await next( context );
    }

    private static async Task<IHtmlDocument?> GetDocumentAsync( IHttpResponseFeature feature, CancellationToken cancellation = default )
    {
        ArgumentNullException.ThrowIfNull( feature );

        using var content = await feature.Response.Content.ReadAsStreamAsync( cancellation )
            .ConfigureAwait( false );

        var document = await BrowsingContext.New()
            .OpenAsync(
                response =>
                {
                    response.Status( feature.Response.StatusCode )
                        .Address( feature.Response.RequestMessage!.RequestUri );

                    var headers = MapHeaders( feature.Response.Content.Headers )
                        .Concat( MapHeaders( feature.Response.Headers ) );

                    foreach( var header in headers )
                    {
                        foreach( string? value in header.Value )
                        {
                            response.Header( header.Key, value );
                        }
                    }

                    content.Seek( 0, SeekOrigin.Begin );
                    response.Content( content );
                },
                cancellation
            ).ConfigureAwait( false );

        return document as IHtmlDocument;
    }

    private static IReadOnlyDictionary<string, StringValues> MapHeaders( HttpHeaders headers )
        => headers.AsEnumerable()
            .ToDictionary(
                entry => entry.Key,
                entry => new StringValues( entry.Value.ToArray() )
            );

    private record HtmlDocumentFeature( IHtmlDocument Document ) : IHtmlDocumentFeature, IDisposable
    {
        public void Dispose( ) => Document?.Dispose();
    }
}