using System.IO.Pipelines;

namespace Earl.Crawler.Templating.Abstractions
{

    public abstract class TemplateResultExecutor<TTemplateResult> : ITemplateResultExecutor
        where TTemplateResult : ITemplateResult
    {

        public abstract Task ExecuteAsync( TTemplateResult result, PipeWriter output, CancellationToken cancellation = default );

        public Task ExecuteAsync( ITemplateResult result, PipeWriter output, CancellationToken cancellation = default )
        {
            if( result is null )
            {
                throw new ArgumentNullException( nameof( result ) );
            }

            if( output is null )
            {
                throw new ArgumentNullException( nameof( output ) );
            }

            if( result is not TTemplateResult typedResult )
            {
                throw new ArgumentException( $"{nameof( ITemplateResult )} of type {result.GetType().Name} was given, but {typeof( TTemplateResult ).Name} was expected." );
            }

            return ExecuteAsync( typedResult, output, cancellation );
        }

    }

}
