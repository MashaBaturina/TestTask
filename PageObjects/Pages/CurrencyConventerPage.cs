using CleverBitTask.PageObjects.AbstractPageObjects;
using CleverBitTask.PageObjects.Components;
using OpenQA.Selenium;

namespace CleverBitTask.PageObjects.Pages
{
    public class CurrencyConventerPage : PageObject
    {
        public MainTabContainerComponent MainTabContainer => new MainTabContainerComponent(webDriver);

        public CurrencyConventerPage(IWebDriver driver) : base(driver)
        {

        }
    }
}
