using System.Text.Json;
using System.Text.Json.Serialization;
using Earl.Crawler.Abstractions;

namespace Earl.Crawler.Persistence.Json.Serialization;

/// <summary> A <see cref="JsonConverter"/> that embeds metadata entry type info. </summary>
public sealed class ResultMetadataConverter : JsonConverter<IResultMetadataCollection>
{
    /// <inheritdoc/>
    /// <exception cref="NotImplementedException"/>
    public override IResultMetadataCollection? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
        => throw new NotImplementedException();

    /// <inheritdoc/>
    public override void Write( Utf8JsonWriter writer, IResultMetadataCollection value, JsonSerializerOptions options )
    {
        ArgumentNullException.ThrowIfNull( writer );
        ArgumentNullException.ThrowIfNull( value );
        ArgumentNullException.ThrowIfNull( options );

        writer.WriteStartArray();

        foreach( object? entry in value )
        {
            if( entry is null )
            {
                continue;
            }

            var type = entry!.GetType();

            writer.WriteStartObject();

            writer.WriteString( "@type", type.AssemblyQualifiedName! );
            WriteEntryProperties( writer, entry, type, options );

            writer.WriteEndObject();
        }

        writer.WriteEndArray();
    }

    private static void WriteEntryProperties( Utf8JsonWriter writer, object entry, Type type, JsonSerializerOptions options )
    {
        var element = JsonSerializer.SerializeToElement( entry, type, options );
        foreach( var property in element.EnumerateObject() )
        {
            property.WriteTo( writer );
        }
    }
}