using Earl.Crawler.Middleware.Http.Abstractions;

namespace Earl.Crawler.Middleware.Http
{

    public record HttpResponseFeature( EarlHttpResponseMessage EarlResponse ) : IHttpResponseFeature, IDisposable
    {
        #region Field
        private bool disposedValue;
        #endregion

        #region Properties

        /// <inheritdoc/>
        public HttpResponseMessage Response => EarlResponse;
        #endregion

        protected virtual void Dispose( bool disposing )
        {
            if( !disposedValue )
            {
                if( disposing )
                {
                    EarlResponse?.Dispose();
                }

                disposedValue = true;
            }
        }

        /// <inheritdoc/>
        public void Dispose( )
        {
            Dispose( disposing: true );
            GC.SuppressFinalize( this );
        }
    }

}
