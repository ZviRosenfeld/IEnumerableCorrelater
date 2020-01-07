using FakeItEasy;
using IEnumerableCorrelater.Interfaces;
using IEnumerableCorrelater.LevenshteinCorrelater;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IEnumerableCorrelater.UnitTests.LevenshteinComparer
{
    [TestClass]
    public class LevenshteinStringCorrelaterTests
    {
        private const int removalCost = 9;
        private const int insertionCost = 10;
        private const int missmatchCost = 11;
        private static readonly IDistanceCalculator<char> distanceCalculator = A.Fake<IDistanceCalculator<char>>();
        private static readonly IStringCorrelater correlater = new LevenshteinStringCorrelater(distanceCalculator, removalCost, insertionCost);

        public LevenshteinStringCorrelaterTests()
        {
            A.CallTo(() => distanceCalculator.Distance(A<char>._, A<char>._))
                .ReturnsLazily((char c1, char c2) => c1 == c2 ? 0 : missmatchCost);
        }

        [TestMethod]
        public void Correlate_Substitution()
        {
            var string1 = "abc";
            var string2 = "aic";

            var expectedResult = new CorrelaterResult<char>(missmatchCost, string1.ToCharArray(), string2.ToCharArray());
            correlater.AssertComparision(string1, string2, expectedResult);
        }

        [TestMethod]
        public void Correlate_Insertion()
        {
            var string1 = "ac";
            var string2 = "abc";

            var expectedResult = new CorrelaterResult<char>(insertionCost, new [] {'a', '\0', 'c'}, string2.ToCharArray());
            correlater.AssertComparision(string1, string2, expectedResult);
        }
    }
}
