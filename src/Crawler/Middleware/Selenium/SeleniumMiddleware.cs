using Earl.Crawler.Middleware.Abstractions;
using Earl.Crawler.Middleware.Selenium.Abstractions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Earl.Crawler.Middleware.Selenium;

public class SeleniumMiddleware : ICrawlerMiddleware
{
    public async Task InvokeAsync( CrawlUrlContext context, CrawlUrlDelegate next )
    {
        if( context is null )
        {
            throw new ArgumentNullException( nameof( context ) );
        }

        if( next is null )
        {
            throw new ArgumentNullException( nameof( next ) );
        }

        var options = new ChromeOptions
        {
            AcceptInsecureCertificates = true
        };

        options.SetLoggingPreference( LogType.Browser, LogLevel.All );
        options.AddArguments(
            "disable-extensions",
            "disable-gpu",
            "headless"
        );

        var driver = new ChromeDriver( ".", options );
        driver.Navigate()
            .GoToUrl( context.Url );

        context.Features.Set<ISeleniumFeature?>( new SeleniumFeature( driver ) );

        await next( context );
        context.Features.Set<ISeleniumFeature?>( null );
    }

    private record SeleniumFeature( IWebDriver Driver ) : ISeleniumFeature, IDisposable
    {
        public void Dispose( )
            => Driver?.Dispose();
    }
}