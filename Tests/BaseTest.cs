using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using WebDriverManager.DriverConfigs.Impl;
using CleverBitTask.Models;
using Microsoft.Extensions.Configuration;
using CleverBitTask.Infrastructure.ConfigurationManager;

namespace CleverBitTask.Tests
{
    public class BaseTest
    {
        protected IWebDriver driver;
        public static CurrencyConverterConfigModel currencyConverterConfigModel;

        [SetUp]
        public virtual void Setup()
        {
            currencyConverterConfigModel = ConfigManager.GetCurrencyConverterConfigModel();
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(currencyConverterConfigModel.BaseUrl);
        }

        [TearDown]
        public virtual void TearDown()
        {
            driver.Quit();
        }
    }
}
