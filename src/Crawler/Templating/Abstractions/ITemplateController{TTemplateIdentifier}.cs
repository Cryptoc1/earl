namespace Earl.Crawler.Templating.Abstractions
{

    public interface ITemplateController<TTemplateIdentifier> : ITemplateController
        where TTemplateIdentifier : TemplateIdentifier, new()
    {
    }

}
