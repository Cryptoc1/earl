﻿using Earl.Crawler.Abstractions;

namespace Earl.Crawler.Configuration;

/// <summary> Describes a builder of <see cref="ICrawlerOptions"/>. </summary>
public interface ICrawlerOptionsBuilder
{
    #region Properties

    /// <summary> A keyable collection of arbitrary data used to build the <see cref="ICrawlerOptions"/>. </summary>
    IDictionary<object, object?> Properties { get; }
    #endregion

    /// <summary> Build the <see cref="ICrawlerOptions"/> for the current state of the builder. </summary>
    ICrawlerOptions Build( );
}