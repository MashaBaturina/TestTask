using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleverBitTask.Infrastructure.RandomGenerators
{
    public static class NumberGenerator
    {
        public static double GetRandomDoubleNumber(double maximum, double minimum = 0.1, double defaultPrecision = 0.01)
        {
            Random random = new Random();
            var randomValue = random.NextDouble() * (maximum - minimum) + minimum;
            var randomValueTruncated = Math.Round(randomValue / defaultPrecision) * defaultPrecision;
            return randomValueTruncated;
        }
    }
}
