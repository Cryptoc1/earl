using System.Diagnostics;

namespace Earl.Crawler.Infrastructure.Http
{

    public class EarlHttpMessageHandler : DelegatingHandler
    {

        /// <inheritdoc/>
        protected override async Task<HttpResponseMessage> SendAsync( HttpRequestMessage request, CancellationToken cancellationToken )
        {
            var stopwatch = ValueStopwatch.StartNew();
            var response = await base.SendAsync( request, cancellationToken )
                .ConfigureAwait( false );

            var elapsed = stopwatch.GetElapsedTime();
            return new EarlHttpResponseMessage
            {
                Content = response.Content,
                ReasonPhrase = response.ReasonPhrase,
                RequestMessage = response.RequestMessage,
                StatusCode = response.StatusCode,
                TotalDuration = elapsed,
                Version = response.Version
            };
        }

        private struct ValueStopwatch
        {
            #region Fields
            private static readonly double TimestampToTicks = TimeSpan.TicksPerSecond / ( double )Stopwatch.Frequency;
            private readonly long startTimestamp;
            #endregion

            #region Properties
            public bool IsActive => startTimestamp is not 0;
            #endregion

            private ValueStopwatch( long startTimestamp )
                => this.startTimestamp = startTimestamp;

            public TimeSpan GetElapsedTime( )
            {
                // Start timestamp can't be zero in an initialized ValueStopwatch. It would have to be literally the first thing executed when the machine boots to be 0.
                // So it being 0 is a clear indication of default(ValueStopwatch)
                if( !IsActive )
                {
                    throw new InvalidOperationException( "An uninitialized, or 'default', ValueStopwatch cannot be used to get elapsed time." );
                }

                var end = Stopwatch.GetTimestamp();
                var timestampDelta = end - startTimestamp;
                var ticks = ( long )( TimestampToTicks * timestampDelta );
                return new TimeSpan( ticks );
            }

            public static ValueStopwatch StartNew( )
                => new( Stopwatch.GetTimestamp() );

        }

    }

}
