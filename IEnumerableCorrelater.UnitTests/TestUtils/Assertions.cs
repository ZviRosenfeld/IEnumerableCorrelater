using System.Collections.Generic;
using System.Linq;
using System.Threading;
using IEnumerableCorrelater.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IEnumerableCorrelater.UnitTests.TestUtils
{
    public static class Assertions
    {
        public static void AssertComparision<T>(this ICorrelater<T> correlater, IEnumerable<T> collection1, IEnumerable<T> collection2, CorrelaterResult<T> expectedResult)
        {
            var result = correlater.Compare(collection1.ToCollectionWrapper(), collection2.ToCollectionWrapper());
            AssertResultIsAsExpected(expectedResult, result);
        }

        private static void AssertResultIsAsExpected<T>(CorrelaterResult<T> expectedResult, CorrelaterResult<T> result)
        {
            if (expectedResult.Distance >= 0)
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

        public static void AssertProgressUpdateWasCalledRightNumberOfTimes(this ICorrelater<string> correlater)
        {
            var array1 = new[] { "A", "D", "B", "C" };
            var array2 = new[] { "A", "B", "D", "C" };

            correlater.AssertProgressUpdateWasCalledRightNumberOfTimes(array1, array2, array1.Length + 1);
        }

        public static void AssertProgressUpdateWasCalledRightNumberOfTimes<T>(this ICorrelater<T> correlater, T[] array1, T[] array2, int expectedProgress)
        {
            var progressUpdates = 0;

            correlater.OnProgressUpdate += (progress, outOf) =>
            {
                Assert.AreEqual(expectedProgress, outOf);
                Interlocked.Increment(ref progressUpdates);
            };
            correlater.Compare(array1.ToCollectionWrapper(), array2.ToCollectionWrapper());

            Assert.AreEqual(expectedProgress, progressUpdates);
        }
    }
}
