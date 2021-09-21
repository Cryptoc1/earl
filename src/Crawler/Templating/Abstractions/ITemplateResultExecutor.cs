using System.IO.Pipelines;

namespace Earl.Crawler.Templating.Abstractions
{

    public interface ITemplateResultExecutor
    {

        Task ExecuteAsync( ITemplateResult result, PipeWriter output, CancellationToken cancellation = default );

    }

}
