using System.ComponentModel.DataAnnotations;

namespace Earl.Crawler.Templating
{

    public class TemplateCrawlHandlerOptions
    {

        [Required]
        public string OutputDirectory { get; set; } = Path.Combine( Environment.CurrentDirectory, "earl_results" );

    }

}
