using System.Collections.Generic;
using System.Linq;
using IEnumerableCorrelater.CollectionWrappers;
using IEnumerableCorrelater.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IEnumerableCorrelater.UnitTests
{
    static class TestUtils
    {
        public static void AssertComparision<T>(this ICorrelater<T> correlater, T[] collection1, T[] collection2, CorrelaterResult<T> expectedResult)
        {
            var result = correlater.Compare(new ArrayCollectionWrapper<T>(collection1), new ArrayCollectionWrapper<T>(collection2));
            AssertResultIsAsExpected(expectedResult, result);
        }
        
        private static void AssertResultIsAsExpected<T>(CorrelaterResult<T> expectedResult, CorrelaterResult<T> result)
        {
            Assert.AreEqual(expectedResult.Distance, result.Distance, "Got wrong distance");
            AssertAreSame(expectedResult.BestMatch1, result.BestMatch1, $"Got wrong {nameof(result.BestMatch1)}");
            AssertAreSame(expectedResult.BestMatch2, result.BestMatch2, $"Got wrong {nameof(result.BestMatch2)}");
        }

        private static void AssertAreSame<T>(IEnumerable<T> collection1, IEnumerable<T> collection2, string message)
        {
            if (collection1.Count() != collection2.Count())
                Assert.Fail(message);

            for (int i = 0; i < collection1.Count(); i++)
            {
                if (collection1.ElementAt(i) == null && collection2.ElementAt(i) != null)
                    Assert.Fail(message);
                else if (collection1.ElementAt(i) != null && !collection1.ElementAt(i).Equals(collection2.ElementAt(i)))
                    Assert.Fail(message);
            }
        }

        public static void AssertAreSame<T>(this ICollectionWrapper<T> collection1, IEnumerable<T> collection2, string message)
        {
            if (collection1.Length != collection2.Count())
                Assert.Fail(message);

            for (int i = 0; i < collection1.Length; i++)
            {
                if (collection1[i] == null && collection2.ElementAt(i) != null)
                    Assert.Fail(message);
                else if (collection1[i] != null && !collection1[i].Equals(collection2.ElementAt(i)))
                    Assert.Fail(message);
            }
        }
    }
}
