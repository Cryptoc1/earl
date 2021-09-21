using Earl.Crawler.Abstractions;
using Earl.Crawler.Templating.Abstractions;
using Earl.Crawler.Templating.DefaultTemplate.Abstractions;
using Earl.Crawler.Templating.DefaultTemplate.ViewModels;
using Earl.Crawler.Templating.Razor;

namespace Earl.Crawler.Templating.DefaultTemplate
{

    public class DefaultTemplateController : RazorTemplateController<DefaultTemplateIdentifier>
    {

        public override async Task<ITemplateResult> InvokeAsync( CrawlUrlResult result, CancellationToken cancellation = default )
        {
            var viewModel = await CreateViewModelAsync( result, cancellation );
            return View( viewModel );
        }

        protected virtual Task<IResultViewModel> CreateViewModelAsync( CrawlUrlResult result, CancellationToken cancellation = default )
        {
            if( result is null )
            {
                throw new ArgumentNullException( nameof( result ) );
            }

            var viewModel = new ResultViewModel( result );
            return Task.FromResult( viewModel as IResultViewModel );
        }

    }

}
