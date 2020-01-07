using FakeItEasy;
using IEnumerableCorrelater.Exceptions;
using IEnumerableCorrelater.Interfaces;
using IEnumerableCorrelater.LevenshteinCorrelater;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IEnumerableCorrelater.UnitTests.LevenshteinCorrelater
{
    [TestClass]
    public class BaseLevenshteinCorrelaterTests
    {
        private const int removalCost = 9;
        private const int insertionCost = 10;
        private const int missmatchCost = 11;
        private static readonly IDistanceCalculator<string> distanceCalculator = A.Fake<IDistanceCalculator<string>>();
        private static readonly BaseLevenshteinCorrelater<string> correlater = new BaseLevenshteinCorrelater<string>(distanceCalculator, removalCost, insertionCost);

        public BaseLevenshteinCorrelaterTests()
        {
            A.CallTo(() => distanceCalculator.Distance(A<string>._, A<string>._))
                .ReturnsLazily((string s1, string s2) => s1.Equals(s2) ? 0 : missmatchCost);
        }

        [TestMethod]
        [ExpectedException(typeof(EnumerableCorrelaterException))]
        public void CorrelateNonNullibleTypes_ThrowException() =>
            new BaseLevenshteinCorrelater<int>(null, removalCost, insertionCost);

        [TestMethod]
        public void CorrelateNullibleTypes_DontThrowException() =>
            new BaseLevenshteinCorrelater<int?>(null, removalCost, insertionCost);

        [TestMethod]
        public void CorrelateChar_DontThrowException() =>
            new BaseLevenshteinCorrelater<char>(null, removalCost, insertionCost);

        [TestMethod]
        public void CorrelateEmptyArrayToFullArray()
        {
            var array1 = new string[0];
            var array2 = new[] { "A", "B", "C" };

            var expectedResult = new CorrelaterResult<string>(insertionCost * array2.Length, new string[3], array2);
            correlater.AssertComparision(array1, array2, expectedResult);
        }

        [TestMethod]
        public void CorrelateFullArrayToEmptyArray()
        {
            var array1 = new[] { "A", "B", "C" };
            var array2 = new string[0];

            var expectedResult = new CorrelaterResult<string>(removalCost * array1.Length, array1, new string[3]);
            correlater.AssertComparision(array1, array2, expectedResult);
        }

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
        public void Correlate_Substitution()
        {
            var array1 = new[] { "A", "D", "C" };
            var array2 = new[] { "A", "B", "C" };

            var expectedResult = new CorrelaterResult<string>(missmatchCost, array1, array2);
            correlater.AssertComparision(array1, array2, expectedResult);
        }
    }
}
