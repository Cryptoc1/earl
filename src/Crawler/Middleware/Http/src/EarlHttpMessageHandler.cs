using System.Diagnostics;
using Earl.Crawler.Middleware.Http.Abstractions;

namespace Earl.Crawler.Middleware.Http;

/// <summary> An <see cref="DelegatingHandler"/> that supports the features of the <see cref="EarlHttpResponseMessage"/>. </summary>
public class EarlHttpMessageHandler : DelegatingHandler
{
    private static readonly Version HandlerVersion = typeof( EarlHttpMessageHandler ).Assembly.GetName().Version!;

    private static void EnsureEarlHeaders( HttpRequestMessage request )
    {
        request.Headers.UserAgent.Add( new( nameof( EarlHttpMessageHandler ), HandlerVersion!.ToString() ) );
        request.Headers.UserAgent.Add( new( "(+https://github.com/cryptoc1/earl)" ) );
    }

    /// <inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync( HttpRequestMessage request, CancellationToken cancellation )
    {
        EnsureEarlHeaders( request );

        var stopwatch = ValueStopwatch.StartNew();
        var response = await base.SendAsync( request, cancellation );

        var elapsed = stopwatch.GetElapsedTime();
        return new EarlHttpResponseMessage
        {
            Content = response.Content,
            ReasonPhrase = response.ReasonPhrase,
            RequestMessage = response.RequestMessage,
            StatusCode = response.StatusCode,
            TotalDuration = elapsed,
            Version = response.Version,
        };
    }

    private readonly struct ValueStopwatch
    {
        public bool IsActive => start is not 0;

        private static readonly long TimestampToTicks = TimeSpan.TicksPerSecond / Stopwatch.Frequency;
        private readonly long start;

        private ValueStopwatch( long start )
            => this.start = start;

        public TimeSpan GetElapsedTime( )
        {
            // Start timestamp can't be zero in an initialized ValueStopwatch. It would have to be literally the first thing executed when the machine boots to be 0.
            // So it being 0 is a clear indication of default(ValueStopwatch)
            if( !IsActive )
            {
                throw new InvalidOperationException( "An uninitialized, or 'default', ValueStopwatch cannot be used to get elapsed time." );
            }

            long now = Stopwatch.GetTimestamp();
            long elapsed = now - start;
            return new TimeSpan( TimestampToTicks * elapsed );
        }

        public static ValueStopwatch StartNew( )
            => new( Stopwatch.GetTimestamp() );
    }
}