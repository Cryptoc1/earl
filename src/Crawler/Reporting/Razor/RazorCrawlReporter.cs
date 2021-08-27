using Earl.Crawler.Abstractions;
using Microsoft.Extensions.Options;
using RazorLight;

namespace Earl.Crawler.Reporting.Razor
{

    public class RazorCrawlReporter : ICrawlReporter
    {
        #region Fields
        private readonly IOptions<RazorCrawlReporterOptions> options;
        private readonly IRazorLightEngine razor;
        #endregion

        public RazorCrawlReporter( IOptions<RazorCrawlReporterOptions> options, IRazorLightEngine razor )
        {
            this.options = options;
            this.razor = razor;
        }

        public async Task OnUrlCrawlComplete( CrawlUrlResult result )
        {
            var report = await razor.CompileRenderAsync( "Views.Test", "Sam" );

            return;
        }

    }

}
