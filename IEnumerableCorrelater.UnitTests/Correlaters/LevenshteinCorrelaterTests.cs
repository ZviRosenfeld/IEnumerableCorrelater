using FakeItEasy;
using IEnumerableCorrelater.Calculators;
using IEnumerableCorrelater.Correlaters;
using IEnumerableCorrelater.Exceptions;
using IEnumerableCorrelater.Interfaces;
using IEnumerableCorrelater.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IEnumerableCorrelater.UnitTests.Correlaters
{
    [TestClass]
    public class LevenshteinCorrelaterTests
    {
        private const int removalCost = 9;
        private const int insertionCost = 10;
        private const int missmatchCost = 11;
        private static readonly LevenshteinCorrelater<string> correlater = new LevenshteinCorrelater<string>(missmatchCost, removalCost, insertionCost);
        
        [TestMethod]
        [ExpectedException(typeof(EnumerableCorrelaterException))]
        public void CorrelateNonNullibleTypes_ThrowException() =>
            new LevenshteinCorrelater<int>(null, removalCost, insertionCost);

        [TestMethod]
        public void CorrelateNullibleTypes_DontThrowException() =>
            new LevenshteinCorrelater<int?>(null, removalCost, insertionCost);

        [TestMethod]
        public void CorrelateChar_DontThrowException() =>
            new LevenshteinCorrelater<char>(null, removalCost, insertionCost);

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

        [TestMethod]
        public void Correlate_Transposition()
        {
            var array1 = new[] { "A", "D", "B", "C" };
            var array2 = new[] { "A", "B", "D", "C" };

            var expectedResult = new CorrelaterResult<string>(removalCost + insertionCost,
                new[] {"A", null, "D", "B", "C"}, new[] {"A", "B", "D", null, "C"});
            correlater.AssertComparision(array1, array2, expectedResult);
        }

        [TestMethod]
        public void DifrentDistnaceCosts()
        {
            var distanceCalculator = A.Fake<IDistanceCalculator<string>>();
            var correlater = new LevenshteinCorrelater<string>(distanceCalculator, 2, 2);
            A.CallTo(() => distanceCalculator.Distance(A<string>._, A<string>._)).ReturnsLazily(
                (string s1, string s2) =>
                {
                    if (s1 == s2) return 0;
                    if (s2 == "A" && s1 == "E") return 1;
                    return 10;
                });


            var array1 = new[] { "V", "E" };
            var array2 = new[] { "T", "A" };

            var expectedResult = new CorrelaterResult<string>(5,
                new[] { null, "V", "E" }, new[] { "T", null, "A" });
            correlater.AssertComparision(array1, array2, expectedResult);
        }

        [TestMethod]
        public void DifrentRemovalCosts1()
        {
            var removalCalculator = A.Fake<IRemovalCalculator<string>>();
            var correlater = new LevenshteinCorrelater<string>(new BasicDistanceCalculator<string>(10), removalCalculator, new BasicInsertionCalculator<string>(10));
            A.CallTo(() => removalCalculator.RemovalCost(A<string>._)).ReturnsLazily(
                (string s) => s == "A" ? 1 : 40);

            var array1 = new[] { "A", "E" };
            var array2 = new[] { "C"};

            var expectedResult = new CorrelaterResult<string>(11, array1, new[] { null, "C" });
            correlater.AssertComparision(array1, array2, expectedResult);
        }

        [TestMethod]
        public void DifrentRemovalCosts2()
        {
            var removalCalculator = A.Fake<IRemovalCalculator<string>>();
            var correlater = new LevenshteinCorrelater<string>(new BasicDistanceCalculator<string>(10), removalCalculator, new BasicInsertionCalculator<string>(10));
            A.CallTo(() => removalCalculator.RemovalCost(A<string>._)).ReturnsLazily(
                (string s) =>
                {
                    if (s == "A") return 1;
                    if (s == "B") return 2;
                    return 3;
                });

            var array1 = new[] { "A", "B" };
            var array2 = new string[0] ;

            var expectedResult = new CorrelaterResult<string>(3, array1, new string[] { null, null });
            correlater.AssertComparision(array1, array2, expectedResult);
        }

        [TestMethod]
        public void DifrentInsertionCosts1()
        {
            var removalCalculator = A.Fake<IInsertionCalculator<string>>();
            var correlater = new LevenshteinCorrelater<string>(new BasicDistanceCalculator<string>(10), new BasicRemovalCalculator<string>(10) ,removalCalculator);
            A.CallTo(() => removalCalculator.InsertionCost(A<string>._)).ReturnsLazily(
                (string s) => s == "A" ? 1 : 40);

            var array1 = new[] { "C" };
            var array2 = new[] { "A", "E" };
            
            var expectedResult = new CorrelaterResult<string>(11, new[] { null, "C" }, array2);
            correlater.AssertComparision(array1, array2, expectedResult);
        }

        [TestMethod]
        public void DifrentInsertionCosts2()
        {
            var removalCalculator = A.Fake<IInsertionCalculator<string>>();
            var correlater = new LevenshteinCorrelater<string>(new BasicDistanceCalculator<string>(10), new BasicRemovalCalculator<string>(10), removalCalculator);
            A.CallTo(() => removalCalculator.InsertionCost(A<string>._)).ReturnsLazily(
                (string s) =>
                {
                    if (s == "A") return 1;
                    if (s == "B") return 2;
                    return 3;
                });

            var array1 = new string[0];
            var array2 = new[] { "A", "B" };

            var expectedResult = new CorrelaterResult<string>(3, new string[] { null, null }, array2);
            correlater.AssertComparision(array1, array2, expectedResult);
        }

        [TestMethod]
        public void SameCollection_AreEquale()
        {
            var array1 = new[] { "A", "B", "C" };
            var array2 = new[] { "A", "B", "C" };

            var expectedResult = new CorrelaterResult<string>(0, array1, array2);
            correlater.AssertComparision(array1, array2, expectedResult);
        }
    }
}
