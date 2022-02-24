namespace Earl.Crawler.Templating.Abstractions;

/// <summary> Represents the identity of a Crawler Template. </summary>
public abstract class TemplateIdentifier
{
    #region Properties

    /// <summary> A unique code name that identfies the template. </summary>
    public abstract string Name { get; }
    #endregion

    /// <inheritdoc/>
    public override string ToString( )
        => Name;
}