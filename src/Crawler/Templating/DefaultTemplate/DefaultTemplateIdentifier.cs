using Earl.Crawler.Templating.Abstractions;

namespace Earl.Crawler.Templating.DefaultTemplate
{

    /// <summary> The identity of the "default" Crawler Template. </summary>
    public sealed class DefaultTemplateIdentifier : TemplateIdentifier
    {

        /// <inheritdoc/>
        public override string Name => "Earl.DefaultTemplate";

    }

}
