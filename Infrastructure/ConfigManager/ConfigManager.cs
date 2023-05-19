using CleverBitTask.Models;
using Microsoft.Extensions.Configuration;

namespace CleverBitTask.Infrastructure.ConfigurationManager
{
    public class ConfigManager
    {
        private static string pathToConfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Config\\CurrencyConverterConfig.json");

        public static CurrencyConverterConfigModel GetCurrencyConverterConfigModel()
        {
            var builder = new ConfigurationBuilder();
            var currencyConverterConfigModel = new CurrencyConverterConfigModel();
            builder.AddJsonFile(pathToConfig);
            IConfigurationRoot configurationRoot = builder.Build();
            configurationRoot.Bind(currencyConverterConfigModel);

            return currencyConverterConfigModel;
        }
    }
}
