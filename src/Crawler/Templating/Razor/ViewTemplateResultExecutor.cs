using System.IO.Pipelines;
using Earl.Crawler.Templating.Abstractions;
using RazorLight;

namespace Earl.Crawler.Templating.Razor
{

    public class ViewTemplateResultExecutor : TemplateResultExecutor<ViewTemplateResult>
    {
        #region Fields
        private readonly IRazorLightEngine razor;
        #endregion

        public ViewTemplateResultExecutor( IRazorLightEngine razor )
            => this.razor = razor;

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

            using var writer = new StreamWriter( output.AsStream(), leaveOpen: true );
            await razor.Handler.RenderTemplateAsync( page, result.Model, writer );

            await output.FlushAsync( cancellation );
        }

    }

}
