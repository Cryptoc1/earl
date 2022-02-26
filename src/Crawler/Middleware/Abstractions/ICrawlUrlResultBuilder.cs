using Earl.Crawler.Abstractions;

namespace Earl.Crawler.Middleware.Abstractions;

public interface ICrawlUrlResultBuilder
{
    #region Properties
    string? DisplayName { get; set; }
    Guid Id { get; }
    IList<object> Metadata { get; }
    #endregion

    CrawlUrlResult Build( );
}