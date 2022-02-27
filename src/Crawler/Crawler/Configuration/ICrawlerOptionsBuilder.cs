using Earl.Crawler.Abstractions.Configuration;

namespace Earl.Crawler.Configuration;

/// <summary> Describes a builder of <see cref="CrawlerOptions"/>. </summary>
public interface ICrawlerOptionsBuilder
{
    #region Properties

    /// <summary> A keyable collection of arbitrary data used to build the <see cref="CrawlerOptions"/>. </summary>
    IDictionary<object, object?> Properties { get; }
    #endregion

    /// <summary> Build the <see cref="CrawlerOptions"/> for the current state of the builder. </summary>
    CrawlerOptions Build( );
}