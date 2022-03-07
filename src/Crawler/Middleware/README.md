# Earl Middleware Layer

The *"Earl Middleware Layer"* refers to a suite of APIs that enable the composition of code into a series of operations performed against a url during a crawl.

> *Earl's Middleware pattern is strongly inlfuenced by ASP.NET Core's Middleware pattern, it is strongly recommended to review ASP.NET Core's [Middleware documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-6.0)*

Middleware accepts a [`CrawlUrlResult`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Middleware/Abstractions/CrawlUrlContext.cs#L9) and a [`CrawlUrlDelegate`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Middleware/Abstractions/CrawlUrlDelegate.cs#L6), the former of which represents the current state of the crawl against the current url; the latter being a reference to the next operation in the pipeline. This behaviour is captured in the [`ICrawlerMiddleware`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Middleware/Abstractions/ICrawlerMiddleware.cs#L5) and is analgous to ASP.NET Core's [`IMiddleware`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.imiddleware) contract.

Middleware is configured for a crawl using the [`Use`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Middleware/Middleware/Configuration/ICrawlerOptionsBuilderMiddlewareExtensions.cs#L7) extension methods, which allow 3 means of implementing middlware:

- Typed Middleware
- Typed Middleware with Options
- Delegate Middleware

### Typed Middleware

*"Typed Middleware"* refers to a class that implements the [`ICrawlerMiddleware`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Middleware/Abstractions/ICrawlerMiddleware.cs#L5) contract, for example:

```csharp
public class CustomMiddleware : ICrawlerMiddleware
{
    public Task InvokeAsync( CrawlUrlContext context, CrawlUrlDelegate next )
    {
        Console.WriteLine( $"Executing typed middleware while crawling {context.Url}" );
        return next( context );
    }
}

// ...

var options = CrawlerOptionsBuilder.CreateDefault()
    .Use<CustomMiddleware>()
    .Build();

await crawler.CrawlAsync( new Uri(...), options );
```

### Typed Middleware with Options

If you wish to allow consumers of Middleware to specify an object to configure the functionality of the Middleware, the [`ICrawlerMiddleware<TOptions>`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Middleware/Abstractions/ICrawlerMiddleware.cs#L16) contract may be used.

When using the [`ICrawlerMiddleware<TOptions>`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Middleware/Abstractions/ICrawlerMiddleware.cs#L16) contract, specify a constructor dependency on an instance of `TOptions`, and invoke the [`Use<TMiddleware, TOptions>( this ICrawlerOptionsBuilder builder, TOptions options )`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Middleware/Middleware/Configuration/ICrawlerOptionsBuilderMiddlewareExtensions.cs#L20) extension method to configure the desired `TOptions` for a crawl: 

```csharp
public record CustomMiddlewareOptions( string Value );

public class CustomMiddleware : ICrawlerMiddleware<CustomMiddlewareOptions>
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
    .Use<CustomMiddleware, CustomMiddlewareOptions>( new( "Hello, World!" ) )
    .Build();

await crawler.CrawlAsync( new Uri(...), options );
```

### Delegate Middleware

The final method of implementing a Middleware is a *"Delegate Middleware"*, which allows an inline delegate method to be used:

```csharp
var options = CrawlerOptionsBuilder.CreateDefault()
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

Delegate Middleware is especially useful for debugging & testing other Middleware in the crawl.