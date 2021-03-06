﻿using IEnumerableCorrelater.Correlaters;
using IEnumerableCorrelater.Exceptions;
using IEnumerableCorrelater.Interfaces;
using IEnumerableCorrelater.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IEnumerableCorrelater.UnitTests.Correlaters
{
    [TestClass]
    public class DynamicLcsCorrelaterTests
    {
        private const int removalCost = 9;
        private const int insertionCost = 10;
        private static readonly ICorrelater<string> correlater = new DynamicLcsCorrelater<string>(removalCost, insertionCost);

        [TestMethod]
        [ExpectedException(typeof(EnumerableCorrelaterException))]
        public void CorrelateNonNullibleTypes_ThrowException() =>
            new DynamicLcsCorrelater<int>();

        [TestMethod]
        public void CorrelateNullibleTypes_DontThrowException() =>
            new DynamicLcsCorrelater<int?>();

        [TestMethod]
        public void Correlate_OneElementToInsert()
        {
            var array1 = new[] { "A", "C" };
            var array2 = new[] { "A", "B", "C" };

            var expectedResult = new CorrelaterResult<string>(insertionCost, new[] { "A", null, "C" }, array2);
            correlater.AssertComparision(array1, array2, expectedResult);
        }

        [TestMethod]
        public void Correlate_OneElementToRemove()
        {
            var array1 = new[] { "A", "B", "C" };
            var array2 = new[] { "A", "C" };

            var expectedResult = new CorrelaterResult<string>(removalCost, array1, new[] { "A", null, "C" });
            correlater.AssertComparision(array1, array2, expectedResult);
        }

        [TestMethod]
        public void Correlate_SameArrays()
        {
            var array = new[] { "A", "B", "C" };

            var expectedResult = new CorrelaterResult<string>(0, array, array);
            correlater.AssertComparision(array, array, expectedResult);
        }

        [TestMethod]
        public void Correlate_TotalMismatch()
        {
            var array1 = new[] { "A", "B", "C" };
            var array2 = new[] { "D", "E", "F" };

            var expectedResult = new CorrelaterResult<string>(3 * (removalCost + insertionCost), new[] { null, null, null, "A", "B", "C" }, new[] { "D", "E", "F", null, null, null });
            correlater.AssertComparision(array1, array2, expectedResult);
        }
    }
}
