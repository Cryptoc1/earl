namespace Earl.Crawler.Infrastructure.Http.Abstractions
{

    /// <summary> Describes a typed <see cref="HttpClient"/> that can make HTTP request during a url crawl. </summary>
    public interface IEarlHttpClient
    {

        Task<EarlHttpResponseMessage> GetAsync( Uri url, CancellationToken cancellation = default );

    }

}
