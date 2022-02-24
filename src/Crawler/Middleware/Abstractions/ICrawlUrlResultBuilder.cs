using Earl.Crawler.Abstractions;

namespace Earl.Crawler.Middleware.Abstractions;

public interface ICrawlUrlResultBuilder
{
    #region Properties
    public string? DisplayName { get; set; }

    public IList<object> Metadata { get; }
    #endregion

    CrawlUrlResult Build( );
}