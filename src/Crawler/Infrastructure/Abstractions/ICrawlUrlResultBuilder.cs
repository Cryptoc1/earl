using Earl.Crawler.Abstractions;

namespace Earl.Crawler.Infrastructure.Abstractions
{

    public interface ICrawlUrlResultBuilder
    {

        #region Properties
        public IList<object> Metadata { get; }

        public string? Title { get; set; }
        #endregion

        CrawlUrlResult Build( );

    }

}
