using Earl.Crawler.Abstractions;

namespace Earl.Crawler.Middleware.Abstractions;

/// <summary> Describes a builder of <see cref="CrawlUrlResult"/>. </summary>
public interface ICrawlUrlResultBuilder
{
    #region Properties
    string? DisplayName { get; set; }
    Guid Id { get; }
    IList<object> Metadata { get; }
    #endregion

    /// <summary> Build the <see cref="CrawlUrlResult"/> for the current state of the builder. </summary>
    CrawlUrlResult Build( );
}