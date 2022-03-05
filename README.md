# Earl 

Earl is a suite of APIs for developing url crawlers & web scrapers driven by a middleware pattern similar to, and strongly influenced by, ASP.NET Core.

## Basic Usage

```csharp
var services = new ServiceCollection()
    .AddLogging()
    .AddEarlCrawler()
    .BuildServiceProvider();

var crawler = services.GetService<IEarlCrawler>();
var options = CrawlerOptionsBuilder.CreateDefault()
    .BatchSize( 50 )
    .MaxRequestCount( 500 )
    .On<CrawlUrlResultEvent>( 
        ( CrawlUrlResultEvent e, CancellationToken cancellation ) =>
        {
            Console.WriteLine( $"Crawled {e.Result.Url}" );
            return ValueTask.CompletedTask;
        }
    )
    .Timeout( TimeSpan.FromMinutes( 30 ) )
    .Use(
        ( CrawlUrlContext context, CrawlUrlDelegate next ) =>
        {
            Console.WriteLine( $"Executing delegate middleware while crawling {context.Url}" );
            return next( context );
        }
    )
    .Build();

await crawler.CrawlAsync( new Uri(...), options );
```

## Documentation

Documentation can be find within the READMEs of the sub-directories representing the conceptual components of Earl:

- [Middleware](https://github.com/Cryptoc1/earl/tree/develop/src/Crawler/Middleware)
- [Persistence](https://github.com/Cryptoc1/earl/tree/develop/src/Crawler/Persistence)

All public APIs *should* contain thorough XML (triple slash) comments. 

> *Something missing, still have questions? Please open an Issue or submit a PR!*