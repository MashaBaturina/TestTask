using CleverBitTask.Infrastructure.ConfigurationManager;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace CleverBitTask.PageObjects.AbstractPageObjects
{
    public abstract class PageObject
    {
        protected IWebDriver webDriver;

        protected Actions actions;

        protected WebDriverWait wait;

        public PageObject(IWebDriver driver)
        {
            webDriver = driver;
            actions = new Actions(driver);

            var timeOut = Double.Parse(ConfigManager.GetCurrencyConverterConfigModel().TimeoutSec);
            wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeOut));
        }
    }
}
