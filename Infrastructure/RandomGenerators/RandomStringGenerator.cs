using System;

namespace CleverBitTask.Infrastructure.RandomGenerators
{
    public static class RandomStringGenerator
    {
        public static string GetRandomString(int length = 3)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var random = new Random();

            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
