# Earl Persistence Layer

The following area contains the *"Earl Persistence Layer"*, which refers to a suite of APIs that enable the results of a crawl to be persisted to a backing storage mechanism. 

Out-of-the-box, and by design, Earl does not collect the results of a crawl. Rather, Earl takes an event based design that leaves it up to the consumer of the [`IEarlCrawler`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Abstractions/IEarlCrawler.cs) contract to handle the [`CrawlUrlResultEvent`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Abstractions/Events/ICrawlerEvents.cs#L41) in order to consume [`CrawlUrlResult`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Abstractions/CrawlUrlResult.cs)s.

At the lowest level of the Crawler API, this is done by specifying an [`ICrawlerEvents`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Abstractions/Events/ICrawlerEvents.cs#L5) implementation for the [`CrawlerOptions.Events`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Abstractions/Configuration/CrawlerOptions.cs#L9) provided to the [`IEarlCrawler`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Abstractions/IEarlCrawler.cs). However, more commonly used, is likely the [`On<TEvent>(this ICrawlerOptionsBuilder builder, CrawlerEventHandler<TEvent> handler)`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Events/Configuration/ICrawlerOptionsBuilderEventExtensions.cs#L13) extension method:

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

The *Earl Persistence Layer* provides an API around such an event handler in order to ease with saving the results of a crawl. The entry point of the Persistence API is the [`PersistTo(this ICrawlerOptionsBuilder builder, Action<ICrawlerPersistenceOptionsBuilder> configure)`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Persistence/Persistence/Configuration/CrawlerOptionsBuilderPersistenceExtensions.cs#L17) extension method. This extension method exposes an [`ICrawlerPersistenceOptionsBuilder`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Persistence/Abstractions/Configuration/ICrawlerPersistenceOptionsBuilder.cs#L4), which serves as an extension point for persistence implementations to offer a fluent syntax for configuration of the backing storage mechanism. 

For example, the *JSON Persistence API* provides the [`ToJson( this ICrawlerPersistenceOptionsBuilder builder, Action<ICrawlerJsonPersistenceOptionsBuilder> configure )`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Persistence/Json/CrawlerPersistenceBuilderJsonExtensions.cs#L12) extension method to configure the persistence of crawl results to JSON files:

```csharp
var options = CrawlerOptionsBuilder.CreateDefault()
    .PersistTo(
        persist => persist.ToJson( json => json.Destination(...) )
    )
    .Build();

await crawler.CrawlAsync( new Uri(...), options );
```

## Behind the Scenes

The *"Persistence Layer"* can be described by the following Types, respectively defined in [`Earl.Crawler.Persistence.Abstractions`](https://github.com/Cryptoc1/earl/tree/develop/src/Crawler/Persistence/Abstractions):
- [`ICrawlerPersistence`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Persistence/Abstractions/ICrawlerPersistence.cs#L6) - *"Describes a service that can persist a [`CrawlUrlResult`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Abstractions/CrawlUrlResult.cs#L8)."*
- [`ICrawlerPersistenceDescriptor`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Persistence/Abstractions/Configuration/ICrawlerPersistenceDescriptor.cs#L4) - *"Describes a type that describes a crawler persistence mechanism."*
- [`ICrawlerPersistenceFactory`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Persistence/Abstractions/ICrawlerPersistenceFactory.cs#L6) - *"Describes a service that can create an instance of an [`ICrawlerPersistence`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Persistence/Abstractions/ICrawlerPersistence.cs#L6) for a given [`ICrawlerPersistenceDescriptor`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Persistence/Abstractions/Configuration/ICrawlerPersistenceDescriptor.cs#L4)."*
- [`ICrawlerPersistenceInvoker`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Persistence/Abstractions/ICrawlerPersistenceInvoker.cs#L7) - *"Describes a service that can invoke the [`ICrawlerPersistence.PersistAsync(CrawlUrlResult, CancellationToken)`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Persistence/Abstractions/ICrawlerPersistence.cs#L11) operation for a given configuration of [`CrawlerPersistenceOptions`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Persistence/Abstractions/Configuration/CrawlerPersistenceOptions.cs#L4)."*

When the [`CrawlUrlResultEvent`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Abstractions/Events/ICrawlerEvents.cs#L41) is emitted, the [`ICrawlerPersistenceInvoker`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Persistence/Abstractions/ICrawlerPersistenceInvoker.cs#L7) is resolved from the event's service provider and invoked to persist the result. The default invoker, [`CrawlerPersistenceInvoker`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Persistence/Persistence/CrawlerPersistenceInvoker.cs#L8), uses the [`ICrawlerPersistenceFactory`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Persistence/Abstractions/ICrawlerPersistenceFactory.cs#L6) to create [`ICrawlerPersistence`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Persistence/Abstractions/ICrawlerPersistence.cs#L6) instances for each of the [`CrawlerPersistenceOptions.Descriptors`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Persistence/Abstractions/Configuration/CrawlerPersistenceOptions.cs#L4) passed to the invoker. The default factory implementation, [`CrawlerPersistenceFactory`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Persistence/Persistence/CrawlerPersistenceFactory.cs#L8), resolves a [`CrawlerPersistenceFactory<TDescriptor>`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Persistence/Persistence/CrawlerPersistenceFactory.cs#L48) implementation from the current service provider for the type of [`ICrawlerPersistenceDescriptor`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Persistence/Abstractions/Configuration/ICrawlerPersistenceDescriptor.cs#L4) passed to the factory. The typed factory is in turn used to create an instance of [`ICrawlerPersistence`](https://github.com/Cryptoc1/earl/blob/develop/src/Crawler/Persistence/Abstractions/ICrawlerPersistence.cs#L6) to persist the result.

