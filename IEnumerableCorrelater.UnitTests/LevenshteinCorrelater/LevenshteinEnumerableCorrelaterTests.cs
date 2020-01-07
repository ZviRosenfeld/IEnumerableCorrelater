using System.Collections.Generic;
using FakeItEasy;
using IEnumerableCorrelater.Interfaces;
using IEnumerableCorrelater.LevenshteinCorrelater;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IEnumerableCorrelater.UnitTests.LevenshteinComparer
{
    [TestClass]
    public class LevenshteinEnumerableCorrelaterTests
    {
        private const int removalCost = 9;
        private const int insertionCost = 10;
        private const int missmatchCost = 11;
        private static readonly IDistanceCalculator<string> distanceCalculator = A.Fake<IDistanceCalculator<string>>();
        private static readonly IEnumerableCorrelater<string> correlater = new LevenshteinEnumerableCorrelater<string>(distanceCalculator, removalCost, insertionCost);

        public LevenshteinEnumerableCorrelaterTests()
        {
            A.CallTo(() => distanceCalculator.Distance(A<string>._, A<string>._))
                .ReturnsLazily((string s1, string s2) => s1.Equals(s2) ? 0 : missmatchCost);
        }
        
        [TestMethod]
        public void Correlate_Substitution()
        {
            var array1 = new[] { "A", "D", "C" };
            var array2 = new[] { "A", "B", "C" };

            var expectedResult = new CorrelaterResult<string>(missmatchCost, array1, array2);
            correlater.AssertComparision(array1, array2, expectedResult);
        }

        [TestMethod]
        public void CorrelateIList_Substitution()
        {
            var list1 = new List<string> { "A", "D", "C" };
            var list2 = new List<string> { "A", "B", "C" };

            var expectedResult = new CorrelaterResult<string>(missmatchCost, list1.ToArray(), list2.ToArray());
            correlater.AssertComparision(list1, list2, expectedResult);
        }
    }
}
