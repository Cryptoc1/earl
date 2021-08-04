using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Earl.Crawler.Infrastructure.Http.Abstractions
{

    public interface IEarlHttpClient
    {

        Task<EarlHttpResponseMessage> GetAsync( Uri url, CancellationToken cancellation = default );

    }

}
