using System;
using IEnumerableCompare.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IEnumerableCompare.UnitTests
{
    static class TestUtils
    {
        public static void AssertComparision<T>(this IEnumerableComparer<T> comparer, T[] array1, T[] array2, CompareResult<T> expectedResult)
        {
            var result = comparer.Compare(array1, array2);
            
            Assert.AreEqual(expectedResult.Distance, result.Distance, "Got wrong distance");
            AssertAreSame(expectedResult.BestMatch1, result.BestMatch1, $"Got wrong {nameof(result.BestMatch1)}");
            AssertAreSame(expectedResult.BestMatch2, result.BestMatch2, $"Got wrong {nameof(result.BestMatch2)}");
        }

        private static void AssertAreSame<T>(T[] array1, T[] array2, string message)
        {
            if (array1.Length != array2.Length)
                Assert.Fail(message);

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] == null && array2[i] != null)
                    Assert.Fail(message);
                else if (array1[i] != null && !array1[i].Equals(array2[i]))
                    Assert.Fail(message);
            }
        }
    }
}
