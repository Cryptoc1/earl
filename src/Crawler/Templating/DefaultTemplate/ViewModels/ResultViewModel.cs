using Earl.Crawler.Abstractions;
using Earl.Crawler.Templating.DefaultTemplate.Abstractions;

namespace Earl.Crawler.Templating.DefaultTemplate.ViewModels;

public record ResultViewModel( CrawlUrlResult Result ) : IResultViewModel
{
    public string Heading => Result.DisplayName;
}