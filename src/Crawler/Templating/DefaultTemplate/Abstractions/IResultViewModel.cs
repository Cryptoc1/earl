using Earl.Crawler.Abstractions;

namespace Earl.Crawler.Templating.DefaultTemplate.Abstractions
{

    public interface IResultViewModel
    {

        string Heading { get; }

        CrawlUrlResult Result { get; }

    }

}
