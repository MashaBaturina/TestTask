using CleverBitTask.Enums;
using CleverBitTask.Infrastructure.Extensions;
using CleverBitTask.PageObjects.AbstractPageObjects;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace CleverBitTask.PageObjects.Components
{
    public class MainTabContainerComponent : PageObject
    {
        private readonly string currencyLocatorPattern = "//span[@class = 'description' and text() = '{0}']//parent::div";

        private const int CurrencyTextStartIndex = 0;

        private const int CurrencyTextEndIndex = 3;

        private By AmountInputLocator = By.XPath("//div[contains(@class, 'amount-input')]/span/Input");

        private By SwapCurrenciesButtonLocator = By.XPath("//button[contains(@aria-label, 'Swap')]");

        private By StraightCurrenciesRatioLocator = By.XPath("//div[contains(@class, 'unit-rates')]/p[1]");

        private By ReverseRatioLocator = By.XPath("//div[contains(@class, 'unit-rates')]/p[2]");

        private By SourseCurrencyLocator = By.XPath("//div[@id = 'midmarketFromCurrency-current-selection']//parent::div[contains(@class, 'ComboboxRootContainer')]");

        private By TargetCurrencyLocator = By.XPath("//div[@id = 'midmarketToCurrency-current-selection']//parent::div[contains(@class, 'ComboboxRootContainer')]");

        private By ConvertButtonLocator = By.XPath("//button[text() = 'Convert']");

        private By ErrorMessageLocator = By.XPath("//div[contains(@class, 'ErrorText')]");

        private By ResultLocator = By.XPath("//p[contains(@class, 'result__BigRate')]");

        private By SourceCurrencyItemDescriptionLocator = By.XPath("//li[contains(@id, 'midmarketFromCurrency')]//div[not(@style)]");

        private By TargetCurrencyItemDescriptionLocator = By.XPath("//li[contains(@id, 'midmarketToCurrency')]//div[not(@style)]");

        private By FromCurrencyListBoxLocator = By.XPath("//ul[@id = 'midmarketFromCurrency-listbox']");

        private By ToCurrencyListBoxLocator = By.XPath("//ul[@id = 'midmarketToCurrency-listbox']");

        public IWebElement ConvertButtonElement => webDriver.FindElement(ConvertButtonLocator);

        public IWebElement ErrorMessageElement => webDriver.FindElement(ErrorMessageLocator);

        public IWebElement ResultElement => webDriver.FindElement(ResultLocator);

        public IWebElement SourceCurrencyElement => webDriver.FindElement(SourseCurrencyLocator);

        public IWebElement TargetCurrencyElement => webDriver.FindElement(TargetCurrencyLocator);

        public IWebElement AmountElement => webDriver.FindElement(AmountInputLocator);

        public IWebElement SwapCurrenciesButton => webDriver.FindElement(SwapCurrenciesButtonLocator);

        public MainTabContainerComponent(IWebDriver driver) : base(driver)
        {
        }

        public void ClickSourceCurrencyAndHold()
        {
            actions.ClickAndHold(SourceCurrencyElement).Perform();
        }

        public void ClickTargetCurrencyAndHold()
        {
            actions.ClickAndHold(TargetCurrencyElement).Perform();
        }

        public void SetAmount(decimal amount)
        {
            AmountElement.Click();
            AmountElement.SendKeys(amount.ToString());
            Thread.Sleep(2000);
        }

        public void SetAmount(string amount)
        {
            AmountElement.Click();
            AmountElement.SendKeys(amount);
            Thread.Sleep(2000);
        }

        public void ClearAmount()
        {
            AmountElement.Click();
            AmountElement.SendKeys(Keys.Control + "A");
            AmountElement.SendKeys(Keys.Delete);
        }

        public void ClickSwapCurrencies()
        {
            SwapCurrenciesButton.Click();
            Thread.Sleep(2000);
        }

        public List<string> GetListOfTopSourceCurrencies(int numberOfCurrenciesFromTop = 2)
        {
            var listOfInitialDescriptions = webDriver.FindElements(SourceCurrencyItemDescriptionLocator).Select(x => x.Text);
            var listOfCurrencies = listOfInitialDescriptions.Select(x => x.Substring(CurrencyTextStartIndex, CurrencyTextEndIndex));

            return listOfCurrencies.Take(numberOfCurrenciesFromTop).ToList();
        }

        public List<string> GetListOfAllSourceCurrenciesExceptTop(int numberOfCurrenciesFromTop = 2)
        {
            var listOfInitialDescriptions = webDriver.FindElements(SourceCurrencyItemDescriptionLocator).Select(x => x.Text);
            var listOfCurrencies = listOfInitialDescriptions.Select(x => x.Substring(CurrencyTextStartIndex, CurrencyTextEndIndex));

            return listOfCurrencies.Skip(numberOfCurrenciesFromTop).ToList();
        }

        public List<string> GetListOfTopTargetCurrencies(int numberOfCurrenciesFromTop = 2)
        {
            var listOfInitialDescriptions = webDriver.FindElements(TargetCurrencyItemDescriptionLocator).Select(x => x.Text);
            var listOfCurrencies = listOfInitialDescriptions.Select(x => x.Substring(CurrencyTextStartIndex, CurrencyTextEndIndex));

            return listOfCurrencies.Take(numberOfCurrenciesFromTop).ToList();
        }

        public List<string> GetListOfAllTargetCurrenciesExceptTop(int numberOfCurrenciesFromTop = 2)
        {
            var listOfInitialDescriptions = webDriver.FindElements(TargetCurrencyItemDescriptionLocator).Select(x => x.Text);
            var listOfCurrencies = listOfInitialDescriptions.Select(x => x.Substring(CurrencyTextStartIndex, CurrencyTextEndIndex));

            return listOfCurrencies.Skip(numberOfCurrenciesFromTop).ToList();
        }

        public void SetSourceCurrency(Currency currency)
        {
            SourceCurrencyElement.Click();

            wait.Until(ExpectedConditions.ElementIsVisible(FromCurrencyListBoxLocator));
            var sourseCurrencySelectElement = webDriver.FindElement(By.XPath(string.Format(currencyLocatorPattern, currency.GetDescription())));
            sourseCurrencySelectElement.Click();
        }

        public void SetTargetCurrency(Currency currency)
        {
            TargetCurrencyElement.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(ToCurrencyListBoxLocator));
            var targetCurrencySelectElement = webDriver.FindElement(By.XPath(string.Format(currencyLocatorPattern, currency.GetDescription())));
            targetCurrencySelectElement.Click();
        }

        public void ClickOnConvertButton()
        {
            ConvertButtonElement.Click();
            wait.Until(ExpectedConditions.ElementIsVisible(ResultLocator));
        }

        public string GetStraightRatioText()
        {
            wait.Until(ExpectedConditions.ElementIsVisible(StraightCurrenciesRatioLocator));
            return webDriver.FindElement(StraightCurrenciesRatioLocator).Text;
        }

        public string GetReverseRatioText()
        {
            wait.Until(ExpectedConditions.ElementIsVisible(ReverseRatioLocator));
            return webDriver.FindElement(ReverseRatioLocator).Text;
        }

        public decimal GetRatioValue()
        {
            var ratioElement = webDriver.FindElement(StraightCurrenciesRatioLocator);
            var initialText = ratioElement.Text;
            var ratioValueText = initialText.Substring(initialText.IndexOf('=')).Split(" ").ElementAt(1);
            var ratioValue = Convert.ToDecimal(ratioValueText);

            return ratioValue;
        }

        public string GetErrorMessageText()
        {
            return ErrorMessageElement.Text;
        }

        public decimal GetMainConvertationResult()
        {
            wait.Until(ExpectedConditions.ElementIsVisible(ResultLocator));
            wait.Until(ExpectedConditions.ElementIsVisible(StraightCurrenciesRatioLocator));
            wait.Until(ExpectedConditions.ElementIsVisible(ReverseRatioLocator));
            var initialText = ResultElement.Text;
            var partialText = initialText.Split(" ").ElementAt(0);
            var mainConvertationResult = Convert.ToDecimal(partialText);

            return mainConvertationResult;
        }

        public string GetSourceCurrencyValue()
        {
            return SourceCurrencyElement.Text.Substring(CurrencyTextStartIndex, CurrencyTextEndIndex);
        }

        public string GetTargetCurrencyValue()
        {
            return TargetCurrencyElement.Text.Substring(CurrencyTextStartIndex, CurrencyTextEndIndex);
        }

        public void ConvertCurrency(Currency sourceCurrency, Currency targetCurrency, decimal amountOfMoney)
        {
            SetSourceCurrency(sourceCurrency);
            SetTargetCurrency(targetCurrency);
            SetAmount(amountOfMoney);
            ClickOnConvertButton();
        }
    }
}
