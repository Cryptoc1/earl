using System.IO.Pipelines;
using Earl.Crawler.Abstractions;
using Earl.Crawler.Templating.Abstractions;
using Microsoft.Extensions.Options;

namespace Earl.Crawler.Templating
{

    public class TemplateCrawlHandler : ICrawlHandler
    {
        #region Fields
        private readonly ITemplateController controller;
        private readonly ITemplateResultExecutor executor;
        private readonly ITemplateNamePolicy namePolicy;
        private readonly IOptions<TemplateCrawlHandlerOptions> options;
        #endregion

        public TemplateCrawlHandler(
            ITemplateController controller,
            ITemplateResultExecutor executor,
            ITemplateNamePolicy namePolicy,
            IOptions<TemplateCrawlHandlerOptions> options
        )
        {
            this.controller = controller;
            this.executor = executor;
            this.namePolicy = namePolicy;
            this.options = options;
        }

        /// <inheritdoc/>
        public async Task OnCrawledUrl( CrawlUrlResult result, CancellationToken cancellation = default )
        {
            if( result is null )
            {
                throw new ArgumentNullException( nameof( result ) );
            }

            var (fileName, fileExtension) = namePolicy.GetNames( result );
            var filePath = Path.Combine( options.Value.OutputDirectory, $"{fileName}.{fileExtension}" );

            using var file = new FileStream( filePath, FileMode.Create, FileAccess.Write );
            var output = PipeWriter.Create( file );

            var template = await controller.InvokeAsync( result, cancellation );
            await executor.ExecuteAsync( template, output, cancellation );

            await output.CompleteAsync();
        }

    }

}
