using IEnumerableCorrelater.Correlaters;
using IEnumerableCorrelater.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IEnumerableCorrelater.UnitTests.Correlaters
{
    [TestClass]
    public class LongestCommonSubsequenceCorrelaterTests
    {
        private const int removalCost = 9;
        private const int insertionCost = 10;
        private static readonly LongestCommonSubsequenceCorrelater<string> correlater = new LongestCommonSubsequenceCorrelater<string>(removalCost, insertionCost);

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
