﻿using Earl.Crawler.Abstractions;
using Earl.Crawler.Persistence.Abstractions.Configuration;

namespace Earl.Crawler.Persistence.Abstractions;

/// <summary> Describes a service that can invoke the <see cref="ICrawlerPersistence.PersistAsync(CrawlUrlResult, CancellationToken)"/> operation for a given configuration of <see cref="CrawlerPersistenceOptions"/>. </summary>
public interface ICrawlerPersistenceInvoker
{
    /// <summary> Invoke the <see cref="ICrawlerPersistence.PersistAsync(CrawlUrlResult, CancellationToken)"/> operation on the given <paramref name="result"/> for the given <paramref name="options"/>. </summary>
    /// <param name="result"> The <see cref="CrawlUrlResult"/> to persist. </param>
    /// <param name="options"> An object representing the configuration of persistence. </param>
    /// <param name="cancellation"> A token that cancels the invocation. </param>
    Task InvokeAsync( CrawlUrlResult result, CrawlerPersistenceOptions options, CancellationToken cancellation = default );
}