using System.Text.Json;
using System.Text.Json.Serialization;

namespace Earl.Crawler.Persistence.Json.Configuration.Tests;

public sealed class CrawlerJsonPersistenceOptionsBuilderExtensionsTests
{
    [Fact]
    public void Configure_adds_build_action_to_builder( )
    {
        static CrawlerJsonPersistenceOptions action( ICrawlerJsonPersistenceOptionsBuilder builder, CrawlerJsonPersistenceOptions options ) => options;
        var builder = new OptionsBuilder()
            .Configure( action );

        Assert.Collection( builder.BuildActions, a => Assert.Equal( action, a ) );
    }

    [Fact]
    public void Serialize_sets_serialization_options( )
    {
        var options = new OptionsBuilder()
            .Serialize( options => options.Converters.Add( new TestConverter() ) )
            .Build();

        Assert.Contains( options.Serialization.Converters, converter => converter is TestConverter );
    }

    private sealed class TestConverter : JsonConverter<Guid>
    {
        public override Guid Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) => throw new NotImplementedException();
        public override void Write( Utf8JsonWriter writer, Guid value, JsonSerializerOptions options ) => throw new NotImplementedException();
    }

    private sealed class OptionsBuilder : ICrawlerJsonPersistenceOptionsBuilder
    {
        public IList<CrawlerJsonPersistenceOptionsBuildAction> BuildActions { get; } = new List<CrawlerJsonPersistenceOptionsBuildAction>();

        public CrawlerJsonPersistenceOptions Build( )
        {
            var options = new CrawlerJsonPersistenceOptions( default!, default! );
            foreach( var action in BuildActions )
            {
                options = action( this, options );
            }

            return options;
        }

        public ICrawlerJsonPersistenceOptionsBuilder Destination( string destination ) => throw new NotImplementedException();
    }
}