using IEnumerableCorrelater.Correlaters;
using IEnumerableCorrelater.Interfaces;
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

            AssertThatLcsIsTheSame(nameof(dynamicResult), nameof(huntResult), dynamicResult, huntResult, string.Join("", collection1), string.Join("", collection2));
            AssertThatLcsIsTheSame(nameof(dynamicResult), nameof(myersResult), dynamicResult, myersResult, string.Join("", collection1), string.Join("", collection2));
        }

        private void AssertThatDistanceIsTheSame(string correlater1Name, string correlater2Name, CorrelaterResult<char> result1, CorrelaterResult<char> result2, string collection1, string collection2)
        {
            var stringForBadResult = new StringBuilder();
            stringForBadResult.AppendLine($"Got diffrent distance: {correlater1Name} = {result1.Distance}; {correlater1Name} = {result2.Distance}");
            stringForBadResult.AppendLine($"Collection1: {collection1}");
            stringForBadResult.AppendLine($"Collection2: {collection2}");
            Assert.AreEqual(result1.Distance, result2.Distance, stringForBadResult.ToString());
        }

        private void AssertThatLcsIsTheSame(string correlater1Name, string correlater2Name, CorrelaterResult<char> result1, CorrelaterResult<char> result2, string collection1, string collection2)
        {
            var result1Lcs = string.Join("", result1.BestMatch1.Where(n => n != '\0'));
            var result2Lcs = string.Join("", result2.BestMatch1.Where(n => n != '\0'));

            var stringForBadResult = new StringBuilder();
            stringForBadResult.AppendLine($"Got diffrent LCS:");
            stringForBadResult.AppendLine($"Result1 LCS: {result1Lcs}");
            stringForBadResult.AppendLine($"Result1 LCS: {result2Lcs}");
            stringForBadResult.AppendLine();
            stringForBadResult.AppendLine($"Collection1: {collection1}");
            stringForBadResult.AppendLine($"Collection2: {collection2}");
            CollectionAssert.AreEqual(result1Lcs.ToCharArray(), result2Lcs.ToCharArray(), stringForBadResult.ToString());
        }
    }
}
