namespace Earl.Crawler.Templating.Abstractions;

public interface ITemplateNamePolicy<TTemplateIdentifier> : ITemplateNamePolicy
    where TTemplateIdentifier : TemplateIdentifier, new()
{
}