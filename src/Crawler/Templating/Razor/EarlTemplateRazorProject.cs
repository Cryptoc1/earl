using Earl.Crawler.Templating.Abstractions;
using RazorLight.Razor;

namespace Earl.Crawler.Templating.Razor;

public class EarlTemplateRazorProject<TTemplateIdentifier> : EmbeddedRazorProject
    where TTemplateIdentifier : TemplateIdentifier, new()
{
    #region Fields
    private readonly TemplateIdentifier identifier;
    #endregion

    public EarlTemplateRazorProject( )
        : base( typeof( TTemplateIdentifier ) )
        => identifier = new TTemplateIdentifier();

    /// <inheritdoc/>
    public override Task<IEnumerable<RazorLightProjectItem>> GetImportsAsync( string key )
        => base.GetImportsAsync( GetTemplateKey( key ) );

    /// <inheritdoc/>
    public override Task<RazorLightProjectItem> GetItemAsync( string key )
        => base.GetItemAsync( GetTemplateKey( key ) );

    private string GetTemplateKey( string key )
    {
        if( key is null )
        {
            throw new ArgumentNullException( nameof( key ) );
        }

        if( !key.StartsWith( identifier.Name ) )
        {
            return key;
        }

        return key[ ( identifier.Name.Length + 1 ).. ];
    }
}