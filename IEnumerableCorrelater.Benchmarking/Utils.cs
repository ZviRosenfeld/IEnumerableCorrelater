using System;
using System.Collections.Generic;

namespace IEnumerableCorrelater.Benchmarking
{
    static class Utils
    {
        private static readonly Random random = new Random();
        private const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static string GetLongString(int length)
        {
            var stringChars = new char[length];
            for (int i = 0; i < length; i++)
                stringChars[i] = CHARS[random.Next(CHARS.Length)];

            return new string(stringChars);
        }

        public static ICollection<char> GetBigCollection(int length)
        {
            var collection = new List<char>();
            for (int i = 0; i < length; i++)
                collection.Add(CHARS[random.Next(CHARS.Length)]);

            return collection;
        }
    }
}
