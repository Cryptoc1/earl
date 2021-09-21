using Earl.Crawler.Abstractions;

namespace Earl.Crawler.Templating.Abstractions
{

    public interface ITemplateController
    {

        Task<ITemplateResult> InvokeAsync( CrawlUrlResult result, CancellationToken cancellation = default );

    }

}
