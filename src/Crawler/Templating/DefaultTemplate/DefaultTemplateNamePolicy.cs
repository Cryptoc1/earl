using Earl.Crawler.Abstractions;
using Earl.Crawler.Templating.Abstractions;

namespace Earl.Crawler.Templating.DefaultTemplate
{

    public class DefaultTemplateNamePolicy : ITemplateNamePolicy<DefaultTemplateIdentifier>
    {

        public (string, string) GetNames( CrawlUrlResult result )
            => (result.Id.ToString(), "html");

    }

}
