# Earl Events

The *"Earl Events Pattern"* refers to the API that allows consumers of an [`IEarlCrawler`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Abstractions/IEarlCrawler.cs) to process information about the executing of a crawl.

Out-of-the-box Earl provides a collection of events that can be found adjacent to the [`ICrawlerEvents`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Abstractions/Events/ICrawlerEvents.cs#L18) contract.

At the lowest level of the Crawler API, events can be handled by specifying an [`ICrawlerEvents`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Abstractions/Events/ICrawlerEvents.cs#L5) implementation for the [`CrawlerOptions.Events`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Abstractions/Configuration/CrawlerOptions.cs#L9) provided to the [`IEarlCrawler`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Abstractions/IEarlCrawler.cs). However, more commonly used, is likely the [`On<TEvent>(this ICrawlerOptionsBuilder builder, CrawlerEventHandler<TEvent> handler)`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Events/Configuration/ICrawlerOptionsBuilderEventExtensions.cs#L13) extension method:

```csharp
var options = CrawlerOptionsBuilder.CreateDefault()
    .On<CrawlUrlResultEvent>(
        async ( CrawlUrlResultEvent e, CancellationToken cancellation ) =>
        {
            // handle the event...
        }
    )
    .Build();

await crawler.CrawlAsync( new Uri(...), options );
```