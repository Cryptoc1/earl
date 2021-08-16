using Earl.Crawler.Infrastructure.Abstractions;
using Earl.Crawler.Infrastructure.Selenium.Abstractions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Earl.Crawler.Infrastructure.Selenium
{

    public class SeleniumMiddleware : ICrawlRequestMiddleware
    {

        public async Task InvokeAsync( CrawlRequestContext context, CrawlRequestDelegate next )
        {
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

}
