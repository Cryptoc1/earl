using Earl.Crawler.Abstractions;
using Earl.Crawler.Infrastructure.Abstractions;

namespace Earl.Crawler
{

    public class CrawlUrlResultBuilder : ICrawlUrlResultBuilder
    {
        #region Fields
        private readonly Guid id;
        private readonly IList<object> metadata;
        private readonly string title;
        private readonly Uri url;
        #endregion

        #region Properties
        /// <inheritdoc/>
        public IList<object> Metadata => metadata;

        /// <inheritdoc/>
        public string? Title { get; set; }
        #endregion

        public CrawlUrlResultBuilder(
            Guid id,
            Uri url,
            IList<object>? metadata = null,
            string? title = null
        )
        {
            this.id = id;
            this.metadata = metadata ?? new List<object>();
            this.title = title ?? $"Crawl Results for '{url}'";
            this.url = url;
        }

        /// <inheritdoc/>
        public virtual CrawlUrlResult Build( )
            => new( id, new ResultMetadataCollection( metadata ), Title ?? title, url );
    }

}
