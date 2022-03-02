namespace Earl.Crawler.Events;

internal static class ValueTaskExtensions
{
    public static async ValueTask WhenAll( params ValueTask[] tasks )
    {
        int count = tasks.Length;
        List<Exception>? exceptions = null;

        foreach( var task in tasks )
        {
            try
            {
                await task.ConfigureAwait( false );
            }
            catch( Exception ex )
            {
                exceptions ??= new List<Exception>( count );
                exceptions.Add( ex );
            }
        }

        if( exceptions is not null )
        {
            throw new AggregateException( exceptions );
        }
    }
}