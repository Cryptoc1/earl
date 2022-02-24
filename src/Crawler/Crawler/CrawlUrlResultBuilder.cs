using Earl.Crawler.Abstractions;
using Earl.Crawler.Middleware.Abstractions;

namespace Earl.Crawler;

public class CrawlUrlResultBuilder : ICrawlUrlResultBuilder
{
    #region Fields
    private readonly string displayName;
    private readonly Guid id;
    private readonly IList<object> metadata;
    private readonly Uri url;
    #endregion

    #region Properties
    /// <inheritdoc/>
    public string? DisplayName { get; set; }

    /// <inheritdoc/>
    public IList<object> Metadata => metadata;
    #endregion

    public CrawlUrlResultBuilder(
        Guid id,
        Uri url,
        string? displayName = null,
        IList<object>? metadata = null
    )
    {
        this.displayName = displayName ?? $"Crawl Results for '{url}'";
        this.id = id;
        this.metadata = metadata ?? new List<object>();
        this.url = url;
    }

    /// <inheritdoc/>
    public virtual CrawlUrlResult Build( )
        => new( DisplayName ?? displayName, id, new ResultMetadataCollection( metadata ), url );
}