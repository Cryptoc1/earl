using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Earl.Crawler.Infrastructure.Http.Abstractions;

namespace Earl.Crawler.Infrastructure.Http
{

    public class EarlHttpClient : IEarlHttpClient
    {
        #region Fields
        private readonly HttpClient client;
        #endregion

        public EarlHttpClient( HttpClient client )
            => this.client = client;

        public async Task<EarlHttpResponseMessage> GetAsync( Uri url, CancellationToken cancellation = default )
            => ( ( await client.GetAsync( url, cancellation ) ) as EarlHttpResponseMessage )!;
    }

}
