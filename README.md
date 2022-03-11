# Earl ![Build Status](https://img.shields.io/github/workflow/status/cryptoc1/earl/default) [![Version](https://img.shields.io/nuget/vpre/Earl.Crawler)](https://www.nuget.org/packages/Earl.Crawler)

Earl is a suite of APIs for developing url crawlers & web scrapers driven by a middleware pattern similar to, and strongly influenced by, ASP.NET Core.

## Basic Usage

```csharp
var services = new ServiceCollection()
    .AddEarlCrawler()
    .AddEarlJsonPersistence()
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
    .PersistTo( persist => persist.ToJson( json => json.Destination(...) ) )
    .Build();

await crawler.CrawlAsync( new Uri(...), options );
```

## Documentation

Documentation can be find within the READMEs of the sub-directories representing the conceptual components of Earl:

- [Events](https://github.com/Cryptoc1/earl/tree/develop/src/Crawler/Events/README.md)
- [Middleware](https://github.com/Cryptoc1/earl/tree/develop/src/Crawler/Middleware/README.md)
- [Persistence](https://github.com/Cryptoc1/earl/tree/develop/src/Crawler/Persistence/README.md)

All public APIs *should* contain thorough XML (triple slash) comments. 

> *Something missing, still have questions? Please open an Issue or submit a PR!*