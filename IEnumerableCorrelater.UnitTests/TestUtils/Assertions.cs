using System.Collections.Generic;
using System.Linq;
using System.Threading;
using IEnumerableCorrelater.Exceptions;
using IEnumerableCorrelater.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IEnumerableCorrelater.UnitTests.TestUtils
{
    public static class Assertions
    {
        public static void AssertComparision<T>(this ICorrelater<T> correlater, IEnumerable<T> collection1, IEnumerable<T> collection2, CorrelaterResult<T> expectedResult)
        {
            var result = correlater.Correlate(collection1, collection2);
            AssertResultIsAsExpected(expectedResult, result);
        }

        public static void AssertOnResultUpdateWorks<T>(this IContinuousCorrelater<T> correlater, IEnumerable<T> collection1, IEnumerable<T> collection2)
        {
            var matchArray1 = new List<T>();
            var matchArray2 = new List<T>();
            correlater.OnResultUpdate += (result) =>
            {
                matchArray1.AddRange(result.BestMatch1);
                matchArray2.AddRange(result.BestMatch2);
            };

            var actualResult = correlater.Correlate(collection1, collection2);
            
            matchArray1.AssertAreSame(actualResult.BestMatch1, "Got wrong updates");
            matchArray2.AssertAreSame(actualResult.BestMatch2, "Got wrong updates");
        }

        public static void AssetThrowsNullElementException<T>(this ICorrelater<T> correlater,
            IEnumerable<T> collection1, IEnumerable<T> collection2, string badCollectionName, int nullIndex)
        {
            try
            {
                correlater.Correlate(collection1, collection2);
                Assert.Fail("Exception wasn't thrown");
            }
            catch (NullElementException e)
            {
                Assert.IsTrue(e.Message.Contains(badCollectionName));
                Assert.IsTrue(e.Message.Contains($"index {nullIndex}"));
            }
        }

        public static void AssertResultIsAsExpected<T>(CorrelaterResult<T> expectedResult, CorrelaterResult<T> result)
        {
            if (expectedResult.Distance >= 0)
                Assert.AreEqual(expectedResult.Distance, result.Distance, "Got wrong distance");
            AssertAreSame(expectedResult.BestMatch1, result.BestMatch1, $"Got wrong {nameof(result.BestMatch1)}");
            AssertAreSame(expectedResult.BestMatch2, result.BestMatch2, $"Got wrong {nameof(result.BestMatch2)}");
        }
        
        public static void AssertAreSame<T>(this IEnumerable<T> collection1, IEnumerable<T> collection2, string message)
        {
            if (collection1 == null && collection2 == null)
                return;

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
                Assert.AreEqual(progressUpdates, progress);
            };
            correlater.Correlate(array1, array2);

            Assert.AreEqual(expectedProgress, progressUpdates);
        }
    }
}
