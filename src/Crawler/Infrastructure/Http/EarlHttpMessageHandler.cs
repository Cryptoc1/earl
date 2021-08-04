using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Earl.Crawler.Infrastructure.Http
{

    public class EarlHttpMessageHandler : DelegatingHandler
    {
        #region Properties
        //public const string StatisticsKey = "earl:statistics";
        #endregion

        protected override async Task<HttpResponseMessage> SendAsync( HttpRequestMessage request, CancellationToken cancellationToken )
        {
            var stopwatch = ValueStopwatch.StartNew();
            var response = await base.SendAsync( request, cancellationToken )
                .ConfigureAwait( false );

            var elapsed = stopwatch.GetElapsedTime();
            /* response.RequestMessage!.Options.Set( new HttpRequestOptionsKey<TimeSpan>( StatisticsKey ), elapsed );
            return response; */

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
            private static readonly double TimestampToTicks = TimeSpan.TicksPerSecond / ( double )Stopwatch.Frequency;

            private long _startTimestamp;

            public bool IsActive => _startTimestamp != 0;

            private ValueStopwatch( long startTimestamp )
            {
                _startTimestamp = startTimestamp;
            }

            public static ValueStopwatch StartNew( ) => new( Stopwatch.GetTimestamp() );

            public TimeSpan GetElapsedTime( )
            {
                // Start timestamp can't be zero in an initialized ValueStopwatch. It would have to be literally the first thing executed when the machine boots to be 0.
                // So it being 0 is a clear indication of default(ValueStopwatch)
                if( !IsActive )
                {
                    throw new InvalidOperationException( "An uninitialized, or 'default', ValueStopwatch cannot be used to get elapsed time." );
                }

                long end = Stopwatch.GetTimestamp();
                long timestampDelta = end - _startTimestamp;
                long ticks = ( long )( TimestampToTicks * timestampDelta );
                return new TimeSpan( ticks );
            }
        }

    }

}
