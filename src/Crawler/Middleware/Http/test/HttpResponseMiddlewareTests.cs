using System.Net;
using Earl.Crawler.Abstractions;
using Earl.Crawler.Middleware.Abstractions;
using Earl.Crawler.Middleware.Http.Abstractions;

namespace Earl.Crawler.Middleware.Http.Tests;

public sealed class HttpResponseMiddlewareTests
{
    [Fact]
    public async Task Middleware_sets_response_feature( )
    {
        using var response = new EarlHttpResponseMessage
        {
            Content = new StringContent( "Hello, World!" ),
            ReasonPhrase = nameof( HttpStatusCode.OK ),
            StatusCode = HttpStatusCode.OK,
            TotalDuration = TimeSpan.Zero,
        };

        var client = new TestClient( _ => Task.FromResult( response ) );
        var middleware = new HttpResponseMiddleware( client );

        await using var features = new CrawlerFeatureCollection();
        var context = new CrawlUrlContext(
            new( default!, CancellationToken.None, default!, default!, default!, default! ),
            features,
            new ResultBuilder(),
            default!,
            default!
        );

        await middleware.InvokeAsync( context, _ => Task.CompletedTask );

        var feature = features.Get<IHttpResponseFeature>();
        Assert.NotNull( feature );
        Assert.Equal( response, feature!.Response );
    }

    private sealed class TestClient : IEarlHttpClient
    {
        private readonly Func<Uri, Task<EarlHttpResponseMessage>> factory;

        public TestClient( Func<Uri, Task<EarlHttpResponseMessage>> factory )
            => this.factory = factory;

        public Task<EarlHttpResponseMessage> GetAsync( Uri url, CancellationToken cancellation = default )
            => factory( url );
    }

    private sealed class ResultBuilder : ICrawlUrlResultBuilder
    {
        public string? DisplayName { get; set; }

        public Guid Id => throw new NotImplementedException();

        public IList<object> Metadata => new List<object>();

        public CrawlUrlResult Build( ) => throw new NotImplementedException();
    }
}