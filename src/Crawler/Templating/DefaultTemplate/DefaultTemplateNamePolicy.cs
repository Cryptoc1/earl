using Earl.Crawler.Abstractions;
using Earl.Crawler.Templating.Abstractions;

namespace Earl.Crawler.Templating.DefaultTemplate
{

    public class DefaultTemplateNamePolicy : ITemplateNamePolicy<DefaultTemplateIdentifier>
    {

        public (string, string) GetNames( CrawlUrlResult result )
        {
            if( result is null )
            {
                throw new ArgumentNullException( nameof( result ) );
            }

            return (result.Id.ToString(), "html");
        }

    }

}
