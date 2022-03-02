# Earl

Earl loves URLs, _crawling them_...

## Usage

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
    );

await crawler.CrawlAsync( new Uri( "..." ), options );
```

### Persisting Results

```chsharp
var services = new ServiceCollection()
    .AddLogging()
    .AddEarlCrawler()
    .AddCrawlerPersistence()
    .AddCrawlerJsonPersistence()

    // ...
    .BuildServiceProvider();

// ...

var options = CrawlerOptionsBuilder.CreateDefault()
    .PersistTo( persist => persist.ToJson( json => json.Destination( "..." ) ) )

    // ...
```

## Design

Earl's implementation is based on _Middleware_. For each url crawled, a pipeline (internally a `Stack<ICrawlerMiddleware>`, see `CrawlerMiddlewareInvoker`) is instantiated and invoked.

_Middleware_ is intended to serve two purposes:

- To expose _Features_ (via `CrawlUrlContext.Features`)
- and to expose _Metadata_ (via `CrawlUrlContext.Metadata`)

_Features_ expose functionality/behavior of a _Middleware_ that may be useful to other _Middleware_ configured further in the middleware pipeline. For example, the `HttpResponseMiddleware` exposes the `IHttpResponseFeature`, which allows further middleware to read the HTTP response made by the crawler, without incurring an additional request. For example, the `UrlScraperMiddleware` depends on the `IHtmlDocumentFeature` provided by the `HtmlDocumentMiddleware`, which in turn depends on the `IHttpResponseFeature`.

_Metadata_ represents the _[serializable]_ results of crawling an URL. This could be any arbitrary data, for example: http timing statistics reported by the `HttpClient` invoked, `console.error`s logged when loading the url in Chrome via Selenium. _Metadata_ is used in the _Templating_ phase to generate visuals meaningful to the user executing the crawl.
