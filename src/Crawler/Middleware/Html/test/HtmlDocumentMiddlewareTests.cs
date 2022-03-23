using System.Net;
using System.Net.Http.Headers;
using Earl.Crawler.Abstractions;
using Earl.Crawler.Middleware.Abstractions;
using Earl.Crawler.Middleware.Html.Abstractions;
using Earl.Crawler.Middleware.Http.Abstractions;

namespace Earl.Crawler.Middleware.Html.Tests;

public sealed class HtmlDocumentMiddlewareTests
{
    [Fact]
    public async Task Middleware_parses_document_from_response_feature( )
    {
        await using var features = new CrawlerFeatureCollection();
        var context = new CrawlUrlContext(
            new( default!, CancellationToken.None, default!, default!, default!, default! ),
            features,
            new ResultBuilder(),
            default!,
            default!
        );

        features.Set<IHttpResponseFeature>( new TestHttpResponseFeature() );

        var middleware = new HtmlDocumentMiddleware();
        await middleware.InvokeAsync( context, _ => Task.CompletedTask );

        var document = context.Features.Get<IHtmlDocumentFeature>()
            ?.Document;

        Assert.NotNull( document );
    }

    [Fact]
    public async Task Middleware_sets_result_displayname_to_document_title( )
    {
        await using var features = new CrawlerFeatureCollection();
        var context = new CrawlUrlContext(
            new( default!, CancellationToken.None, default!, default!, default!, default! ),
            features,
            new ResultBuilder(),
            default!,
            default!
        );

        features.Set<IHttpResponseFeature>( new TestHttpResponseFeature() );

        var middleware = new HtmlDocumentMiddleware();
        await middleware.InvokeAsync( context, _ => Task.CompletedTask );

        Assert.Equal( "Hello, World!", context.Result.DisplayName );
    }

    private sealed class ResultBuilder : ICrawlUrlResultBuilder
    {
        public string? DisplayName { get; set; }

        public Guid Id => throw new NotImplementedException();

        public IList<object> Metadata => throw new NotImplementedException();

        public CrawlUrlResult Build( ) => throw new NotImplementedException();
    }

    private sealed class TestHttpResponseFeature : IHttpResponseFeature, IDisposable
    {
        public HttpResponseMessage Response { get; }

        private static readonly string Content = @"<!DOCTYPE html>
<html>
<head>
<title>Hello, World!</title>
</head>
<body>
<h1>Hello, World!</h1>
<p>
Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
</p>
</body>
</html>
";

        public TestHttpResponseFeature( )
        {
            var content = new StringContent( Content );
            content.Headers.ContentType = new MediaTypeHeaderValue( "text/html" );

            Response = new HttpResponseMessage
            {
                Content = content,
                ReasonPhrase = nameof( HttpStatusCode.OK ),
                StatusCode = HttpStatusCode.OK,
            };
        }

        public void Dispose( ) => Response.Dispose();
    }
}