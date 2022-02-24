using Earl.Crawler.Abstractions;
using Earl.Crawler.Templating.Abstractions;

namespace Earl.Crawler.Templating.Razor;

public abstract class RazorTemplateController<TTemplateIdentifier> : ITemplateController<TTemplateIdentifier>
    where TTemplateIdentifier : TemplateIdentifier, new()
{
    #region Fields
    private readonly TemplateIdentifier identifier;
    #endregion

    protected RazorTemplateController( )
        => identifier = new TTemplateIdentifier();

    public abstract Task<ITemplateResult> InvokeAsync( CrawlUrlResult result, CancellationToken cancellation = default );

    protected ViewTemplateResult View<TModel>( TModel? model )
        => View( RazorTemplateViewNames.Result, model );

    protected ViewTemplateResult View<TModel>( string viewName, TModel? model )
        => new()
        {
            Model = model,
            ViewName = $"{identifier.Name}.{viewName}"
        };
}