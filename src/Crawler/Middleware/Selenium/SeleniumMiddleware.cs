using Earl.Crawler.Middleware.Abstractions;
using Earl.Crawler.Middleware.Selenium.Abstractions;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace Earl.Crawler.Middleware.Selenium;

public class SeleniumMiddleware : ICrawlerMiddleware
{
    public async Task InvokeAsync( CrawlUrlContext context, CrawlUrlDelegate next )
    {
        ArgumentNullException.ThrowIfNull( context );
        ArgumentNullException.ThrowIfNull( next );

        var options = new EdgeOptions
        {
            AcceptInsecureCertificates = true
        };

        options.SetLoggingPreference( LogType.Browser, LogLevel.All );
        options.AddArguments(
            "disable-extensions",
            "disable-gpu",
            "headless"
        );

        var driver = new EdgeDriver( ".", options );
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