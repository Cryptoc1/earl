using Earl.Crawler.Persistence.Abstractions.Configuration;

namespace Earl.Crawler.Persistence.Abstractions;

/// <summary> Describes a service that can create an instance of an <see cref="ICrawlerPersistence"/> for a given <see cref="ICrawlerPersistenceDescriptor"/>. </summary>
public interface ICrawlerPersistenceFactory
{
    /// <summary> Create an <see cref="ICrawlerPersistence"/> for the given <paramref name="descriptor"/>. </summary>
    /// <param name="descriptor"> The descriptor to be used to create an instance of an <see cref="ICrawlerPersistence"/>. </param>
    ICrawlerPersistence Create( ICrawlerPersistenceDescriptor descriptor );
}