using System.Runtime.CompilerServices;
using AngleSharp.Html.Dom;
using Earl.Crawler.Middleware.UrlScraping.Abstractions;
using Earl.Crawler.Middleware.UrlScraping.Abstractions.Configuration;

namespace Earl.Crawler.Middleware.UrlScraping;

/// <summary> Default implementation of <see cref="IUrlScraperInvoker"/>. </summary>
public sealed class UrlScraperInvoker : IUrlScraperInvoker
{
    private readonly IUrlScraperFactory factory;

    public UrlScraperInvoker( IUrlScraperFactory factory )
        => this.factory = factory;

    /// <inheritdoc/>
    public async IAsyncEnumerable<Uri> InvokeAsync( IReadOnlyList<IUrlScraperDescriptor> scrapers, IHtmlDocument document, [EnumeratorCancellation] CancellationToken cancellation = default )
    {
        ArgumentNullException.ThrowIfNull( scrapers );
        ArgumentNullException.ThrowIfNull( document );

        foreach( var scraper in scrapers.Select( factory.Create ) )
        {
            await foreach( var url in scraper.ScrapeAsync( document, cancellation ).ConfigureAwait( false ) )
            {
                yield return url;
            }
        }
    }
}