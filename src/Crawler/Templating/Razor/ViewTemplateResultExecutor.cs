using System.IO.Pipelines;
using Earl.Crawler.Templating.Abstractions;
using Microsoft.IO;
using RazorLight;
using WebMarkupMin.Core;
using WebMarkupMin.NUglify;

namespace Earl.Crawler.Templating.Razor
{

    public class ViewTemplateResultExecutor : TemplateResultExecutor<ViewTemplateResult>
    {
        #region Fields
        private readonly IRazorLightEngine razor;
        private readonly RecyclableMemoryStreamManager streamManager;
        #endregion

        public ViewTemplateResultExecutor(
            IRazorLightEngine razor,
            RecyclableMemoryStreamManager streamManager
        )
        {
            this.razor = razor;
            this.streamManager = streamManager;
        }

        public override async Task ExecuteAsync( ViewTemplateResult result, PipeWriter output, CancellationToken cancellation = default )
        {
            if( result is null )
            {
                throw new ArgumentNullException( nameof( result ) );
            }

            if( output is null )
            {
                throw new ArgumentNullException( nameof( output ) );
            }

            var page = await razor.Handler.CompileTemplateAsync( result.ViewName );

            using var memoryStream = streamManager.GetStream();
            using var memoryWriter = new StreamWriter( memoryStream, leaveOpen: true );

            await razor.Handler.RenderTemplateAsync( page, result.Model, memoryWriter );
            await memoryWriter.FlushAsync();

            memoryStream.Seek( 0, SeekOrigin.Begin );

            using var reader = new StreamReader( memoryStream );
            var content = await reader.ReadToEndAsync();

            using var writer = new StreamWriter( output.AsStream(), leaveOpen: true );
            var minifier = new HtmlMinifier(
                new()
                {
                    MinifyInlineCssCode = true,
                    MinifyInlineJsCode = true,
                    RemoveHtmlComments = true,
                    RemoveHtmlCommentsFromScriptsAndStyles = true,
                    RemoveOptionalEndTags = false,
                    RemoveTagsWithoutContent = true
                },
                cssMinifier: new NUglifyCssMinifier(),
                jsMinifier: new NUglifyJsMinifier()
            );

            var minification = minifier.Minify( content );
            await writer.WriteAsync( minification.MinifiedContent );

            await output.FlushAsync( cancellation );
        }

    }

}
