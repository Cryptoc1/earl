using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Earl.Crawler.Infrastructure.UrlScraper.Abstractions
{

    public interface IUrlScraperFilter
    {

        Task<IEnumerable<Uri>> FilterAsync( IEnumerable<Uri> urls, CancellationToken cancellation = default );

    }

}
