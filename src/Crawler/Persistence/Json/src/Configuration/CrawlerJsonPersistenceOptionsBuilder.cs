using System.Text.Json;
using Earl.Crawler.Persistence.Json.Serialization;

namespace Earl.Crawler.Persistence.Json.Configuration;

/// <summary> Default implementation of <see cref="ICrawlerJsonPersistenceOptionsBuilder"/>. </summary>
public sealed class CrawlerJsonPersistenceOptionsBuilder : ICrawlerJsonPersistenceOptionsBuilder
{
    /// <inheritdoc/>
    public IList<CrawlerJsonPersistenceOptionsBuildAction> BuildActions { get; } = new List<CrawlerJsonPersistenceOptionsBuildAction>();

    private string? destination;

    /// <inheritdoc/>
    public CrawlerJsonPersistenceOptions Build( )
    {
        if( string.IsNullOrWhiteSpace( destination ) )
        {
            throw new ArgumentNullException( nameof( Destination ) );
        }

        var options = new CrawlerJsonPersistenceOptions( destination, new JsonSerializerOptions( JsonSerializerDefaults.General ) );
        foreach( var buildAction in BuildActions )
        {
            options = buildAction( this, options );
        }

        return options;
    }

    /// <summary> Creates an <see cref="ICrawlerJsonPersistenceOptionsBuilder"/> with the default configuration. </summary>
    public static ICrawlerJsonPersistenceOptionsBuilder CreateDefault( )
        => new CrawlerJsonPersistenceOptionsBuilder()
            .Serialize( ConfigureDefaultSerialization );

    private static void ConfigureDefaultSerialization( JsonSerializerOptions options )
    {
        ArgumentNullException.ThrowIfNull( options );
        options.Converters.Add( new ResultMetadataConverter() );
    }

    /// <inheritdoc/>
    public ICrawlerJsonPersistenceOptionsBuilder Destination( string destination )
    {
        this.destination = destination;
        return this;
    }
}