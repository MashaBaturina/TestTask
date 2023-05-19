namespace CleverBitTask.Infrastructure.Calculator
{
    public static class CurrencyConversionChecker
    {
        public static bool IsCurrencyConvertedCorrectly(decimal initialAmount, decimal ratio, decimal currentAmount, decimal defaultPrecision = 0.01m)
        {
            var roundCurrentAmount = Math.Round(currentAmount / defaultPrecision) * defaultPrecision;
            var calculatedValue = initialAmount * ratio;
            decimal result = Math.Round(calculatedValue / defaultPrecision) * defaultPrecision;

            return result == roundCurrentAmount;
        }
    }
}
