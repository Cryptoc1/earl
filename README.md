# Earl 

Earl is a suite of APIs for developing url crawlers & web scrapers driven by a middleware pattern similar to, and strongly influenced by, ASP.NET Core.


## Usage

### Getting Started

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

await crawler.CrawlAsync( new Uri(...), options );
```

### Typed Middleware

```csharp
public class CustomMiddleware : IMiddleware
{
    public Task InvokeAsync( CrawlUrlContext context, CrawlUrlDelegate next )
    {
        Console.WriteLine( $"Executing typed middleware while crawling {context.Url}" );
        return next( context );
    }
}

// ...

var options = CrawlerOptionsBuilder.CreateDefault()
    .Use<CustomMiddleware>();

await crawler.CrawlAsync( new Uri(...), options );
```

### Typed Middleware with Options

```csharp
public record CustomMiddlewareOptions( string Value );

public class CustomMiddleware : IMiddleware<CustomMiddlewareOptions>
{
    private readonly CustomMiddlewareOptions options;

    // Accept options as ctor dependency
    public CustomMiddleware( CustomMiddlewareOptions options )
        => this.options = options;

    public Task InvokeAsync( CrawlUrlContext context, CrawlUrlDelegate next )
    {
        Console.WriteLine( $"Executing typed middleware with option '{options.Value}' while crawling {context.Url}" );
        return next( context );
    }
}

// ...

var options = CrawlerOptionsBuilder.CreateDefault()
    .Use<CustomMiddleware, CustomMiddlewareOptions>( new( "Hello, World!" );

await crawler.CrawlAsync( new Uri(...), options );
```

### Delegate Middleware

```csharp
var options = CrawlerOptionsBuilder.CreateDefault()
    .Use(
        ( CrawlUrlContext context, CrawlUrlDelegate next ) =>
        {
            Console.WriteLine( $"Executing delegate middleware while crawling {context.Url}" );
            return next( context );
        }
    );

await crawler.CrawlAsync( new Uri(...), options );
```

### Persisting Results

```csharp
var services = new ServiceCollection()
    .AddLogging()
    .AddEarlCrawler()
    .AddEarlJsonPersistence()

    // ...
    .BuildServiceProvider();

// ...

var options = CrawlerOptionsBuilder.CreateDefault()
    .PersistTo( persist => persist.ToJson( json => json.Destination( "..." ) ) )

    // ...
```