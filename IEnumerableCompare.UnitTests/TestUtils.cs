using IEnumerableCompare.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IEnumerableCompare.UnitTests
{
    static class TestUtils
    {
        public static void AssertComparision<T>(this IEnumerableComparer<T> comparer, T[] array1, T[] array2, int expectedDistance)
        {
            var result = comparer.Compare(array1, array2);

            Assert.AreEqual(expectedDistance, result.Distance);
        }
    }
}
