using Earl.Crawler.Abstractions.Events;

namespace Earl.Crawler.Events;

/// <summary> Describes a method that handles a <see cref="CrawlEvent"/> of type <typeparamref name="TEvent"/>. </summary>
/// <typeparam name="TEvent"> The type of <see cref="CrawlEvent"/> handled. </typeparam>
/// <param name="e"> The <typeparamref name="TEvent"/> to be handled. </param>
/// <param name="cancellation"> A token that cancels the event. </param>
/// <seealso cref="ICrawlerEvents"/>
/// <seealso cref="CrawlEvent"/>
public delegate ValueTask CrawlEventHandler<TEvent>( TEvent e, CancellationToken cancellation )
    where TEvent : CrawlEvent;