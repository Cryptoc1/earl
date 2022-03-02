using System.Text.Json;

namespace Earl.Crawler.Persistence.Json.Configuration;

/// <summary> Represents the configuration of persistence. </summary>
/// <param name="Destination"> The absolute path to save JSON results to. </param>
/// <param name="Serialization"> The <see cref="JsonSerializerOptions"/> to use. </param>
public record CrawlerJsonPersistenceOptions( string Destination, JsonSerializerOptions Serialization );