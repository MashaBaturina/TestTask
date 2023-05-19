using CleverBitTask.Enums;
using CleverBitTask.Infrastructure.Calculator;
using CleverBitTask.Infrastructure.Constants;
using CleverBitTask.Infrastructure.RandomGenerators;
using CleverBitTask.PageObjects.Pages;

namespace CleverBitTask.Tests
{
    [TestFixture]
    public class CurrencyConverterTests : BaseTest
    {
        [Test]
        public void Conversion_Calculation_With_Numeric_Money_Amount()
        {
            var amountOfMoney = (decimal)NumberGenerator.GetRandomDoubleNumber(maximum: 10);
            var sourceCurrency = Currency.EUR;
            var targetCurrency = Currency.USD;

            var currencyConverterPage = new CurrencyConventerPage(driver);
            currencyConverterPage.MainTabContainer.ConvertCurrency(sourceCurrency, targetCurrency, amountOfMoney);

            var mainConvertationResult = currencyConverterPage.MainTabContainer.GetMainConvertationResult();
            var ratioValue = currencyConverterPage.MainTabContainer.GetRatioValue();
            
            var isConversionCorrect = CurrencyConversionChecker.IsCurrencyConvertedCorrectly(amountOfMoney, ratioValue, mainConvertationResult);

            Assert.IsTrue(isConversionCorrect, "The conversion is not calculated correctly.");
        }

        [Test]
        public void Conversion_Rates_For_Single_Units_Are_Displayed_After_Conversion_Is_Done()
        {
            var amountOfMoney = (decimal)NumberGenerator.GetRandomDoubleNumber(maximum: 10);
            var sourceCurrency = Currency.EUR;
            var targetCurrency = Currency.USD;

            var currencyConverterPage = new CurrencyConventerPage(driver);
            currencyConverterPage.MainTabContainer.ConvertCurrency(sourceCurrency, targetCurrency, amountOfMoney);

            var straightRatioText = currencyConverterPage.MainTabContainer.GetStraightRatioText();
            var reverseRatioText = currencyConverterPage.MainTabContainer.GetReverseRatioText();

            Assert.IsTrue(straightRatioText != String.Empty, "Straight ratio is not displayed.");
            Assert.IsTrue(reverseRatioText != String.Empty, "Reversed ratio is not displayed.");
        }

        [Test]
        public void Conversion_Is_Happening_If_Source_Currency_Amount_Is_Set_To_Negative_Value()
        {
            var sourceCurrency = Currency.EUR;
            var targetCurrency = Currency.USD;
            var amountOfMoney = (decimal)NumberGenerator.GetRandomDoubleNumber(maximum: 10);
            var negativeAmountOfMoney = -(decimal)NumberGenerator.GetRandomDoubleNumber(maximum: 10);
            var expectedErrorMessage = ErrorMessagesConstants.EnterTheAmountGreaterThenZeroSourceCurrencyAmountErrorMessage;

            var currencyConverterPage = new CurrencyConventerPage(driver);

            currencyConverterPage.MainTabContainer.ConvertCurrency(sourceCurrency, targetCurrency, amountOfMoney);
            currencyConverterPage.MainTabContainer.ClearAmount();
            currencyConverterPage.MainTabContainer.SetAmount(negativeAmountOfMoney);

            var mainConvertationResult = currencyConverterPage.MainTabContainer.GetMainConvertationResult();
            var ratioValue = currencyConverterPage.MainTabContainer.GetRatioValue();

            var isConversionCorrect = CurrencyConversionChecker.IsCurrencyConvertedCorrectly(negativeAmountOfMoney, ratioValue, mainConvertationResult);

            Assert.IsTrue(isConversionCorrect, "The conversion is not correct.");

            var currentText = currencyConverterPage.MainTabContainer.GetErrorMessageText();

            Assert.That(currentText, Is.EqualTo(expectedErrorMessage));
        }

        [Test]
        public void Chosen_Currencies_Are_Contained_In_Page_Url()
        {
            var currencyConventerFullUrlFragment = currencyConverterConfigModel.BaseUrl + "/convert/?Amount={0}&From={1}&To={2}";
            var sourceCurrency = Currency.EUR;
            var targetCurrency = Currency.USD;
            var amountOfMoney = (decimal)NumberGenerator.GetRandomDoubleNumber(maximum: 10);

            var currencyConverterPage = new CurrencyConventerPage(driver);
            currencyConverterPage.MainTabContainer.ConvertCurrency(sourceCurrency, targetCurrency, amountOfMoney);
            var currentUrl = driver.Url;

            var expectedUrl = string.Format(currencyConventerFullUrlFragment, amountOfMoney, Currency.EUR, Currency.USD);
            Assert.That(currentUrl, Is.EqualTo(expectedUrl), "Currencies are not contained in the page url.");

            currencyConverterPage.MainTabContainer.ClickSwapCurrencies();

            expectedUrl = string.Format(currencyConventerFullUrlFragment, amountOfMoney, Currency.USD, Currency.EUR);

            currentUrl = driver.Url;

            Assert.That(currentUrl, Is.EqualTo(expectedUrl), "Currencies are not displayed in a correct order in the url after swap.");
        }

        [Test]
        public void User_Can_Navigate_To_Specific_Currencies_Conversion_Via_Direct_Link()
        {
            var directLinkOfUsdToEurConversion = currencyConverterConfigModel.BaseUrl + "/convert/?Amount=100&From=USD&To=EUR";

            driver.Navigate().GoToUrl(directLinkOfUsdToEurConversion);
            var currencyConverterPage = new CurrencyConventerPage(driver);
            var sourceCurrency = currencyConverterPage.MainTabContainer.GetSourceCurrencyValue();
            var targetCurrency = currencyConverterPage.MainTabContainer.GetTargetCurrencyValue();

            Assert.That(sourceCurrency, Is.EqualTo(Currency.USD.ToString()), "Source currency is not correct.");
            Assert.That(targetCurrency, Is.EqualTo(Currency.EUR.ToString()), "Target currency is not correct.");
        }

        [Test]
        public void Error_Message_Is_Displayed_If_Non_Numeric_Value_Is_Specified()
        {
            var sourceCurrency = Currency.EUR;
            var targetCurrency = Currency.USD;
            var amountOfMoney = (decimal)NumberGenerator.GetRandomDoubleNumber(maximum: 10);
            var randomString = RandomStringGenerator.GetRandomString();
            var expectedErrorMessage = ErrorMessagesConstants.EnterTheValidSourceCurrencyAmountErrorMessage;

            var currencyConverterPage = new CurrencyConventerPage(driver);
            currencyConverterPage.MainTabContainer.ConvertCurrency(sourceCurrency, targetCurrency, amountOfMoney);
            currencyConverterPage.MainTabContainer.ClearAmount();
            currencyConverterPage.MainTabContainer.SetAmount(randomString);

            var currentErrorMessage = currencyConverterPage.MainTabContainer.GetErrorMessageText();

            Assert.That(currentErrorMessage, Is.EqualTo(expectedErrorMessage));
        }

        [Test]
        public void Coversion_Recalculates_After_Swap()
        {
            var amountOfMoney = (decimal)NumberGenerator.GetRandomDoubleNumber(maximum: 10);
            var initialSourceCurrency = Currency.EUR;
            var initialTargetCurrency = Currency.USD;

            var currencyConverterPage = new CurrencyConventerPage(driver);
            currencyConverterPage.MainTabContainer.ConvertCurrency(initialSourceCurrency, initialTargetCurrency, amountOfMoney);
            var initialMainConvertationResult = currencyConverterPage.MainTabContainer.GetMainConvertationResult();
            currencyConverterPage.MainTabContainer.ClickSwapCurrencies();
            var mainConvertationResultAfterSwap = currencyConverterPage.MainTabContainer.GetMainConvertationResult();
            var currentSourceCurrency = currencyConverterPage.MainTabContainer.GetSourceCurrencyValue();
            var currentTargetCurrency = currencyConverterPage.MainTabContainer.GetTargetCurrencyValue();

            Assert.That(currentSourceCurrency, Is.EqualTo(initialTargetCurrency.ToString()));
            Assert.That(currentTargetCurrency, Is.EqualTo(initialSourceCurrency.ToString()));
            Assert.That(mainConvertationResultAfterSwap, Is.Not.EqualTo(initialMainConvertationResult));
        }

        [Test]
        public void Popular_Currencies_Are_Displayed_At_The_Top()
        {
            var expectedListOfTopCurrencies = new List<string> { Currency.USD.ToString(), Currency.EUR.ToString() };

            var currencyConverterPage = new CurrencyConventerPage(driver);

            currencyConverterPage.MainTabContainer.ClickSourceCurrencyAndHold();
            var listOfTopCurrenciesFromSourceCurrenciesList = currencyConverterPage.MainTabContainer.GetListOfTopSourceCurrencies();

            Assert.That(listOfTopCurrenciesFromSourceCurrenciesList, Is.EqualTo(expectedListOfTopCurrencies), "Top currencies are not displayed at the top of the source currencies dropdown.");

            currencyConverterPage.MainTabContainer.ClickTargetCurrencyAndHold();
            var listOfTopCurrenciesFromTargetCurrenciesList = currencyConverterPage.MainTabContainer.GetListOfTopTargetCurrencies();

            Assert.That(listOfTopCurrenciesFromTargetCurrenciesList, Is.EqualTo(expectedListOfTopCurrencies), "Top currencies are not displayed at the top of the target currencies dropdown");
        }
   
        [Test]
        public void All_Currencies_Except_Top_Are_Displayed_In_Alphabetical_Order()
        {
            var expectedListOfTopCurrencies = new List<string> { Currency.USD.ToString(), Currency.EUR.ToString() };

            var currencyConverterPage = new CurrencyConventerPage(driver);

            currencyConverterPage.MainTabContainer.ClickSourceCurrencyAndHold();
            var listOfAllSourceCurrenciesExceptTop = currencyConverterPage.MainTabContainer.GetListOfAllSourceCurrenciesExceptTop();
            var orderedListOfAllSourceCurrenciesExceptTop = listOfAllSourceCurrenciesExceptTop.OrderBy(item => item);

            Assert.IsTrue(listOfAllSourceCurrenciesExceptTop.SequenceEqual(orderedListOfAllSourceCurrenciesExceptTop), "Source currencies are not ordered alphabetically.");

            currencyConverterPage.MainTabContainer.ClickTargetCurrencyAndHold();
            var listOfTopCurrenciesFromTargetCurrenciesList = currencyConverterPage.MainTabContainer.GetListOfAllTargetCurrenciesExceptTop();
            var orderedListOfAllTargetCurrenciesExceptTop = listOfTopCurrenciesFromTargetCurrenciesList.OrderBy(item => item);

            Assert.IsTrue(listOfTopCurrenciesFromTargetCurrenciesList.SequenceEqual(orderedListOfAllTargetCurrenciesExceptTop), "Source currencies are not ordered alphabetically.");
        }
    }
}