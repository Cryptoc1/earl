using OpenQA.Selenium;

namespace Earl.Crawler.Middleware.Selenium.Abstractions
{

    public interface ISeleniumFeature
    {

        IWebDriver Driver { get; }

    }

}
