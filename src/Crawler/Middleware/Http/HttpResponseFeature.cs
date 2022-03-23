using Earl.Crawler.Middleware.Http.Abstractions;

namespace Earl.Crawler.Middleware.Http;

/// <summary> Default implementation of <see cref="IHttpResponseFeature"/>. </summary>
/// <param name="EarlResponse"> <see cref="IHttpResponseFeature.Response"/>. </param>
public record HttpResponseFeature( EarlHttpResponseMessage EarlResponse ) : IHttpResponseFeature, IDisposable
{
    /// <inheritdoc/>
    public HttpResponseMessage Response => EarlResponse;

    private bool disposedValue;

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