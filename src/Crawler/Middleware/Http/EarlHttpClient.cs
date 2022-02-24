using Earl.Crawler.Middleware.Http.Abstractions;

namespace Earl.Crawler.Middleware.Http;

public class EarlHttpClient : IEarlHttpClient
{
    #region Fields
    private readonly HttpClient client;
    #endregion

    public EarlHttpClient( HttpClient client )
        => this.client = client;

    /// <inheritdoc/>
    public async Task<EarlHttpResponseMessage> GetAsync( Uri url, CancellationToken cancellation = default )
    {
        var response = await client.GetAsync( url, HttpCompletionOption.ResponseContentRead, cancellation );
        return ( response as EarlHttpResponseMessage )!;
    }
}