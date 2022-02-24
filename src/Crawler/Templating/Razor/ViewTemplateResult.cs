using Earl.Crawler.Templating.Abstractions;

namespace Earl.Crawler.Templating.Razor;

public class ViewTemplateResult : ITemplateResult
{
    public object? Model { get; init; }

    public string ViewName { get; init; }
}