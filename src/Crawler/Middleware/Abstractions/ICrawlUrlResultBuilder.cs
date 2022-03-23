using Earl.Crawler.Abstractions;

namespace Earl.Crawler.Middleware.Abstractions;

/// <summary> Describes a builder of <see cref="CrawlUrlResult"/>. </summary>
public interface ICrawlUrlResultBuilder
{
    string? DisplayName { get; set; }
    Guid Id { get; }
    IList<object> Metadata { get; }

    /// <summary> Build the <see cref="CrawlUrlResult"/> for the current state of the builder. </summary>
    CrawlUrlResult Build( );
}