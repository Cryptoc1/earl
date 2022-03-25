using Earl.Crawler.Middleware;
using Earl.Crawler.Middleware.Html;
using Earl.Crawler.Middleware.Http;

namespace Earl.Crawler.Configuration.Tests;

public sealed class CrawlerOptionsBuilderTests
{
    [Fact]
    public void Builder_builds_options_with_build_actions( )
    {
        var builder = CrawlerOptionsBuilder.CreateDefault();

        bool built = false;
        builder.BuildActions.Add(
            ( _, options ) =>
            {
                built = true;
                return options;
            }
        );

        builder.Build();
        Assert.True( built );
    }

    [Fact]
    public void Builder_uses_processor_count_for_default_max_parallelism( )
    {
        var options = CrawlerOptionsBuilder.CreateDefault().Build();
        Assert.Equal( Environment.ProcessorCount, options.MaxDegreeOfParallelism );
    }

    [Fact]
    public void Builder_uses_processor_count_to_the_power_of_two_for_default_batch_size( )
    {
        var options = CrawlerOptionsBuilder.CreateDefault().Build();
        Assert.Equal( Math.Pow( Environment.ProcessorCount, 2 ), options.BatchSize );
    }

    [Fact]
    public void Default_builder_uses_html_middleware( )
    {
        var options = CrawlerOptionsBuilder.CreateDefault().Build();
        var descriptor = options.Middleware.SingleOrDefault(
            descriptor => descriptor is ServiceCrawlerMiddlewareDescriptor serviceDescriptor && serviceDescriptor.MiddlewareType == typeof( HtmlDocumentMiddleware )
        );

        Assert.NotNull( descriptor );
    }

    [Fact]
    public void Default_builder_uses_http_middleware( )
    {
        var options = CrawlerOptionsBuilder.CreateDefault().Build();
        var descriptor = options.Middleware.SingleOrDefault(
            descriptor => descriptor is ServiceCrawlerMiddlewareDescriptor serviceDescriptor && serviceDescriptor.MiddlewareType == typeof( HttpResponseMiddleware )
        );

        Assert.NotNull( descriptor );
    }
}