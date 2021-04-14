using IEnumerableCorrelater.Correlaters;
using IEnumerableCorrelater.Interfaces;
using IEnumerableCorrelater.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEnumerableCorrelater.UnitTests.Correlaters
{
    /// <summary>
    /// This class tests that all LCS correlaters return the same best match
    /// </summary>
    [TestClass]
    public class LcsCorrelatersAreTheSameTests
    {
        private const int STRING_LEGNT = 1_000;
        private const int TEST_RUNS = 20;

        ICorrelater<char> huntSzymanskiCorrelater = new HuntSzymanskiCorrelater<char>();
        ICorrelater<char> myersAlgorithmCorrelater = new MyersAlgorithmCorrelater<char>();
        ICorrelater<char> dynamicLcsCorrelater = new DynamicLcsCorrelater<char>();

        [TestMethod]
        public void AlomostSameCollections_TestThatAllLcsCorrelatersReturnIdenticalResults()
        {
            for (var i = 0; i < TEST_RUNS; i++)
            {
                var collection1 = TestUtils.Utils.GetLongString(STRING_LEGNT);
                var collection2 = collection1.ToList();
                collection2[500] = collection1.ElementAt(500) == 'a' ? 'b' : 'a';
                
                AssertResultsForAllCorrelatersAreTheSame(collection1, collection2);
            }
        }

        [TestMethod]
        public void TotalyDifferentStrings_TestThatAllLcsCorrelatersReturnIdenticalResults()
        {
            for (var i = 0; i < TEST_RUNS; i++)
            {
                var collection1 = TestUtils.Utils.GetLongString(STRING_LEGNT);
                var collection2 = TestUtils.Utils.GetLongString(STRING_LEGNT);

                AssertResultsForAllCorrelatersAreTheSame(collection1, collection2);
            }
        }

        private void AssertResultsForAllCorrelatersAreTheSame(IEnumerable<char> collection1, IEnumerable<char> collection2)
        {
            var huntResult = huntSzymanskiCorrelater.Correlate(collection1, collection2);
            var myersResult = myersAlgorithmCorrelater.Correlate(collection1, collection2);
            var dynamicResult = dynamicLcsCorrelater.Correlate(collection1, collection2);

            AssertThatDistanceIsTheSame(nameof(dynamicResult), nameof(huntResult), dynamicResult, huntResult, string.Join("", collection1), string.Join("", collection2));
            AssertThatDistanceIsTheSame(nameof(dynamicResult), nameof(myersResult), dynamicResult, myersResult, string.Join("", collection1), string.Join("", collection2));

            AssertThatLcsIsTheSameLength(nameof(dynamicResult), nameof(huntResult), dynamicResult, huntResult, string.Join("", collection1), string.Join("", collection2));
            AssertThatLcsIsTheSameLength(nameof(dynamicResult), nameof(myersResult), dynamicResult, myersResult, string.Join("", collection1), string.Join("", collection2));
        }

        private void AssertThatDistanceIsTheSame(string correlater1Name, string correlater2Name, CorrelaterResult<char> result1, CorrelaterResult<char> result2, string collection1, string collection2)
        {
            var stringForBadResult = new StringBuilder();
            stringForBadResult.AppendLine($"Got diffrent distance: {correlater1Name} = {result1.Distance}; {correlater2Name} = {result2.Distance}");
            stringForBadResult.AppendLine($"Collection1: {collection1}");
            stringForBadResult.AppendLine($"Collection2: {collection2}");
            Assert.AreEqual(result1.Distance, result2.Distance, stringForBadResult.ToString());
        }

        /// <summary>
        /// While there's no guarantee that the LCS are the same, they should at least be the same length
        /// </summary>
        private void AssertThatLcsIsTheSameLength(string correlater1Name, string correlater2Name, CorrelaterResult<char> result1, CorrelaterResult<char> result2, string collection1, string collection2)
        {
            var result1Lcs = result1.GetLcsFromResult();
            var result2Lcs = result2.GetLcsFromResult();

            var stringForBadResult = new StringBuilder();
            stringForBadResult.AppendLine($"Got diffrent LCS lengths:");
            stringForBadResult.AppendLine($"{correlater1Name} LCS: {string.Join(",", result1Lcs)}");
            stringForBadResult.AppendLine($"{correlater2Name} LCS: {string.Join(",", result2Lcs)}");
            stringForBadResult.AppendLine();
            stringForBadResult.AppendLine($"Collection1: {collection1}");
            stringForBadResult.AppendLine($"Collection2: {collection2}");

            Assert.AreEqual(result1Lcs.Count(), result2Lcs.Count(), stringForBadResult.ToString());
        }
    }
}
