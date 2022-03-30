using System.Text.Json;
using Earl.Crawler.Abstractions;

namespace Earl.Crawler.Persistence.Json.Serialization.Tests;

public sealed class ResultMetadataConverterTests
{
    [Fact]
    public async ValueTask Converter_embeds_type_info( )
    {
        var metadata = new MetadataCollection(
            new[]
            {
                new Data(),
                new Data(),
                new Data(),
            }
        );

        await using var stream = new MemoryStream();
        await using var writer = new Utf8JsonWriter( stream, new() { Indented = true } );

        var converter = new ResultMetadataConverter();
        converter.Write( writer, metadata, new( JsonSerializerDefaults.General ) );

        await writer.FlushAsync();
        stream.Seek( 0, SeekOrigin.Begin );

        using var reader = new StreamReader( stream );
        Assert.Equal(
            $@"[
  {{
    ""@type"": ""Earl.Crawler.Persistence.Json.Serialization.Tests.ResultMetadataConverterTests+Data, Earl.Crawler.Persistence.Json.Tests, Version = 0.0.0.0, Culture = neutral, PublicKeyToken = null""
  }},
  {{
    ""@type"": ""Earl.Crawler.Persistence.Json.Serialization.Tests.ResultMetadataConverterTests+Data, Earl.Crawler.Persistence.Json.Tests, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
  }},
  {{
    ""@type"": ""Earl.Crawler.Persistence.Json.Serialization.Tests.ResultMetadataConverterTests+Data, Earl.Crawler.Persistence.Json.Tests, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
  }}
]",
            await reader.ReadToEndAsync()
        );
    }

    private sealed class MetadataCollection : IResultMetadataCollection
    {
        public int Count => metadata.Count;

        private readonly IReadOnlyList<object> metadata;

        public MetadataCollection( IReadOnlyList<object> metadata )
            => this.metadata = metadata;

        public object this[ int index ] => metadata[ index ];

        public IEnumerator<object> GetEnumerator( ) => metadata.GetEnumerator();

        public T? GetMetadata<T>( )
            where T : class => throw new NotImplementedException();

        public IReadOnlyList<T> GetOrderedMetadata<T>( )
            where T : class => throw new NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator( ) => GetEnumerator();
    }

    private sealed record Data( );
}