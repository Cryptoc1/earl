using System.Net;
using Earl.Crawler.Middleware.Http.Abstractions;

namespace Earl.Crawler.Middleware.Http.Tests;

public sealed class EarlHttpMessageHandlerTests
{
    [Fact]
    public async Task Handlers_adds_user_agent_to_request( )
    {
        using var handler = new EarlHttpMessageHandler
        {
            InnerHandler = new TestHandler(),
        };

        using var invoker = new HttpMessageInvoker( handler );

        using var request = new HttpRequestMessage( HttpMethod.Get, new Uri( "https://localhost:8080/earl" ) );
        using var response = await invoker.SendAsync( request, CancellationToken.None );

        Assert.Contains(
            $"{nameof( EarlHttpMessageHandler )}/{typeof( EarlHttpMessageHandler ).Assembly.GetName().Version} (+https://github.com/cryptoc1/earl)",
            response.RequestMessage!.Headers.UserAgent.ToString()
        );
    }

    [Theory]
    [InlineData( 0 )]
    [InlineData( 75 )]
    [InlineData( 100 )]
    [InlineData( 150 )]
    [InlineData( 500 )]
    [InlineData( 1500 )]
    public async Task Handler_measures_message_duration_with_33_percent_accuracy( int duration )
    {
        using var handler = new EarlHttpMessageHandler
        {
            InnerHandler = new TestHandler( duration ),
        };

        using var invoker = new HttpMessageInvoker( handler );

        using var request = new HttpRequestMessage( HttpMethod.Get, new Uri( "https://localhost:8080/earl" ) );
        using var response = Assert.IsType<EarlHttpResponseMessage>(
            await invoker.SendAsync( request, CancellationToken.None )
        );

        Assert.NotEqual( default, response.TotalDuration );

        double inaccuracy = Math.Max( duration, 1 ) * .333;
        Assert.InRange( response.TotalDuration.TotalMilliseconds, duration, duration + inaccuracy );
    }

    [Fact]
    public async Task Handler_returns_typed_response( )
    {
        using var handler = new EarlHttpMessageHandler
        {
            InnerHandler = new TestHandler(),
        };

        using var invoker = new HttpMessageInvoker( handler );

        using var request = new HttpRequestMessage( HttpMethod.Get, new Uri( "https://localhost:8080/earl" ) );
        using var response = await invoker.SendAsync( request, CancellationToken.None );
        Assert.IsType<EarlHttpResponseMessage>( response );
    }

    private sealed class TestHandler : HttpMessageHandler
    {
        private readonly int duration;

        public TestHandler( int duration = 0 )
            => this.duration = duration;

        protected override async Task<HttpResponseMessage> SendAsync( HttpRequestMessage request, CancellationToken cancellationToken )
        {
            await Task.Delay( duration, cancellationToken );
            return new HttpResponseMessage
            {
                Content = new StringContent( "Hello, World!" ),
                ReasonPhrase = nameof( HttpStatusCode.OK ),
                RequestMessage = request,
                StatusCode = HttpStatusCode.OK,
            };
        }
    }
}