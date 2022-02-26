using Earl.Crawler.Abstractions;
using Earl.Crawler.Middleware.Abstractions;

namespace Earl.Crawler;

/// <summary> Default implementation of <see cref="ICrawlUrlResultBuilder"/>. </summary>
public class CrawlUrlResultBuilder : ICrawlUrlResultBuilder
{
    #region Fields
    private readonly string displayName;
    private readonly Uri url;
    #endregion

    #region Properties

    /// <inheritdoc/>
    public string? DisplayName { get; set; }

    /// <inheritdoc/>
    public Guid Id { get; }

    /// <inheritdoc/>
    public IList<object> Metadata { get; }
    #endregion

    public CrawlUrlResultBuilder( Uri url, string? displayName = null, IList<object>? metadata = null )
    {
        this.displayName = displayName ?? $"Crawl Results for '{url}'";
        this.url = url;

        Id = Guid.NewGuid();
        Metadata = metadata ?? new List<object>();
    }

    /// <inheritdoc/>
    public virtual CrawlUrlResult Build( )
        => new( DisplayName ?? displayName, Id, new ResultMetadataCollection( Metadata ), url );
}