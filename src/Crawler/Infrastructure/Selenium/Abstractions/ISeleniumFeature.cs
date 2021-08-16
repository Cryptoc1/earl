using OpenQA.Selenium;

namespace Earl.Crawler.Infrastructure.Selenium.Abstractions
{

    public interface ISeleniumFeature
    {

        IWebDriver Driver { get; }

    }

}
