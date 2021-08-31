namespace Earl.Crawler.Reporting.Templating
{

    public class TemplateCrawlHandlerOptions
    {

        public string OutputDirectory { get; set; } = Path.Combine( Environment.CurrentDirectory, "earl_results" );

    }

}
