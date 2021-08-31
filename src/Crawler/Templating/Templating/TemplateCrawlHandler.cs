using Earl.Crawler.Abstractions;
using Earl.Crawler.Templating.Abstractions;
using Microsoft.Extensions.Options;
using RazorLight;

namespace Earl.Crawler.Reporting.Templating
{

    public class TemplateCrawlHandler : ICrawlHandler
    {
        #region Fields
        private readonly IOptions<TemplateCrawlHandlerOptions> options;
        private readonly IRazorLightEngine razor;
        #endregion

        public TemplateCrawlHandler( IOptions<TemplateCrawlHandlerOptions> options, IRazorLightEngine razor )
        {
            this.options = options;
            this.razor = razor;
        }

        /// <inheritdoc/>
        public async Task OnCrawlUrlResult( CrawlUrlResult result, CancellationToken cancellation = default )
        {
            if( result is null )
            {
                throw new ArgumentNullException( nameof( result ) );
            }

            var outputDirectory = options.Value.OutputDirectory;
            if( string.IsNullOrWhiteSpace( outputDirectory ) )
            {
                throw new InvalidOperationException( $"{nameof( TemplateCrawlHandlerOptions.OutputDirectory )} is required." );
            }

            using var output = new StreamWriter( Path.Combine( options.Value.OutputDirectory, $"{result.Id}.html" ), false );
            var report = await razor.CompileRenderAsync( TemplatingStrings.ReportViewName, result );

            await output.WriteAsync( report.AsMemory(), cancellation );
            await output.FlushAsync();
        }

    }

}
