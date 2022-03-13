using System.Text.Json;
using System.Text.Json.Serialization;
using Earl.Crawler.Abstractions;

namespace Earl.Crawler.Persistence.Json.Serialization;

/// <summary> A <see cref="JsonConverter"/> that embeds metadata entry type info. </summary>
public class ResultMetadataConverter : JsonConverter<IResultMetadataCollection>
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

        foreach( var entry in value.Select( _ => new MetadataEntry( _ ) ) )
        {
            JsonSerializer.Serialize( writer, entry, entry.GetType(), options );
        }

        writer.WriteEndArray();
    }

    private struct MetadataEntry
    {
        #region Fields
        private readonly Type type;
        #endregion

        #region Property

        [JsonPropertyName( "@type" )]
        public string Type => type.AssemblyQualifiedName!;

        public object Value { get; }
        #endregion

        public MetadataEntry( object value )
        {
            ArgumentNullException.ThrowIfNull( value );

            type = value.GetType();
            Value = value;
        }
    }
}