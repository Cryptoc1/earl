using Earl.Crawler.Abstractions;

namespace Earl.Crawler.Templating.Abstractions;

public interface ITemplateNamePolicy
{
    (string, string) GetNames( CrawlUrlResult result );
}