using Earl.Crawler.Abstractions;

namespace Earl.Crawler.Templating.Abstractions;

public interface ITemplateNamePolicy
{
    (string Name, string Extension) GetNames( CrawlUrlResult result );
}