namespace Earl.Crawler.Templating.Razor;

public static class RazorLightDependencyBuilderExtensions
{
    /* private static IServiceCollection? Services( RazorLightDependencyBuilder builder )
        => typeof( RazorLightDependencyBuilder )
            .GetField( "_services", BindingFlags.Instance | BindingFlags.NonPublic )
            ?.GetValue( builder ) as IServiceCollection;

    public static RazorLightDependencyBuilder UseEarlTemplateProject<TTemplateIdentifier>( this RazorLightDependencyBuilder builder )
        where TTemplateIdentifier : TemplateIdentifier, new()
    {
        if( builder is null )
        {
            throw new ArgumentNullException( nameof( builder ) );
        }

        var services = Services( builder );
        if( services is null )
        {
            throw new InvalidOperationException( $"Unable to retieve {nameof( IServiceCollection )} from the {nameof( RazorLightDependencyBuilder )}." );
        }

        services.RemoveAll<RazorLightProject>();
        services.AddSingleton<RazorLightProject>( new EarlTemplateRazorProject<TTemplateIdentifier>() );

        return builder;
    } */
}