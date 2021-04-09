using FakeItEasy;
using IEnumerableCorrelater.Correlaters;
using IEnumerableCorrelater.CorrelaterWrappers;
using IEnumerableCorrelater.Interfaces;
using IEnumerableCorrelater.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IEnumerableCorrelater.UnitTests.CorrelaterWrappers
{
    [TestClass]
    public class IgnoreIdenticalBeginningAndEndCorrelaterWrapperTests
    {
        private const int removalInsertionCost = 7;
        private const int missmatchCost = 10;
        private static readonly ICorrelater<char> innerCorrelater = A.Fake<ICorrelater<char>>();
        private static readonly IgnoreIdenticalBeginningAndEndCorrelaterWrapper<char> correlater =
            new IgnoreIdenticalBeginningAndEndCorrelaterWrapper<char>(innerCorrelater);
        private static readonly IgnoreIdenticalBeginningAndEndCorrelaterWrapper<char> levenshteinCorrelater =
            new IgnoreIdenticalBeginningAndEndCorrelaterWrapper<char>(new LevenshteinCorrelater<char>(missmatchCost, removalInsertionCost, removalInsertionCost));

        [TestMethod]
        public void SameString_CallInnderCorrelaterWithEmptyCollection()
        {
            var s = "abcdefg";
            A.CallTo(() => innerCorrelater.Correlate(A<IEnumerable<char>>._, A<IEnumerable<char>>._, A<CancellationToken>._)).
                Invokes((IEnumerable<char> collection1, IEnumerable<char> collection2, CancellationToken ct) => 
            {
                Assert.IsTrue(!collection1.Any(), $"{nameof(collection1)} was not empty");
                Assert.IsTrue(!collection2.Any(), $"{nameof(collection2)} was not empty");
            });
            correlater.Correlate(s, s);
        }

        [TestMethod]
        public void DifferentMiddle_CallInnderCorrelaterOnlyMiddle()
        {
            var s1 = "abc1234efg";
            var s2 = "abc5678efg";

            A.CallTo(() => innerCorrelater.Correlate(A<IEnumerable<char>>._, A<IEnumerable<char>>._, A<CancellationToken>._)).
                Invokes((IEnumerable<char> collection1, IEnumerable<char> collection2, CancellationToken ct) =>
            {
                collection1.AssertAreSame("1234", nameof(collection1));
                collection2.AssertAreSame("5678", nameof(collection2));
            });
            correlater.Correlate(s1, s2);
        }

        [TestMethod]
        public void SameString_ReturnRightMatch()
        {
            var s = "abcdefg";

            var expectedResult = new CorrelaterResult<char>(0, s.ToArray(), s.ToArray());
            levenshteinCorrelater.AssertComparision(s, s, expectedResult);
        }

        [TestMethod]
        public void DifferentLastChar_ReturnRightMatch()
        {
            var s1 = "abcdefg";
            var s2 = "abcdefh";

            var expectedResult = new CorrelaterResult<char>(missmatchCost, s1.ToArray(), s2.ToArray());
            levenshteinCorrelater.AssertComparision(s1, s2, expectedResult);
        }

        [TestMethod]
        public void MissingLastChar_ReturnRightMatch()
        {
            var s1 = "abcdefg";
            var s2 = "abcdef";

            var expectedResult = new CorrelaterResult<char>(removalInsertionCost, s1.ToArray(), "abcdef\0".ToArray());
            levenshteinCorrelater.AssertComparision(s1, s2, expectedResult);
        }

        [TestMethod]
        public void DifferentFirstChar_ReturnRightMatch()
        {
            var s1 = "abcdefg";
            var s2 = "zbcdefg";

            var expectedResult = new CorrelaterResult<char>(missmatchCost, s1.ToArray(), s2.ToArray());
            levenshteinCorrelater.AssertComparision(s1, s2, expectedResult);
        }

        [TestMethod]
        public void MissingFirstChar_ReturnRightMatch()
        {
            var s1 = "abcdefg";
            var s2 = "bcdefg";

            var expectedResult = new CorrelaterResult<char>(removalInsertionCost, s1.ToArray(), "\0bcdefg".ToArray());
            levenshteinCorrelater.AssertComparision(s1, s2, expectedResult);
        }

        [TestMethod]
        public void DifferentMiddleChar_ReturnRightMatch()
        {
            var s1 = "abcdefg";
            var s2 = "abc8efg";

            var expectedResult = new CorrelaterResult<char>(missmatchCost, s1.ToArray(), s2.ToArray());
            levenshteinCorrelater.AssertComparision(s1, s2, expectedResult);
        }

        [TestMethod]
        public void MissingMiddleChar_ReturnRightMatch()
        {
            var s1 = "abcdefg";
            var s2 = "abcefg";

            var expectedResult = new CorrelaterResult<char>(removalInsertionCost, s1.ToArray(), "abc\0efg".ToArray());
            levenshteinCorrelater.AssertComparision(s1, s2, expectedResult);
        }

        [TestMethod]
        public void DifferentString_ReturnRightMatch()
        {
            var s1 = "abcd";
            var s2 = "efgh";

            var expectedResult = new CorrelaterResult<char>(missmatchCost * 4, s1.ToArray(), s2.ToArray());
            levenshteinCorrelater.AssertComparision(s1, s2, expectedResult);
        }

        [TestMethod]
        [DataRow("abc1234def", "abc5678def", 2)] // Diff in the middle
        [DataRow("1234def", "5678def", 1)] // Diff at the beginning
        [DataRow("abc1234", "abc5678", 2)] // Diff at the end
        [DataRow("1234", "5678", 1)] // Diff eveywhere
        public void IContinuousCorrelaterTest_InnerCorrelaterNotContinuous(string string1, string string2, int timeToCallOrResultUpdate)
        {
            A.CallTo(() => innerCorrelater.Correlate(A<IEnumerable<char>>._, A<IEnumerable<char>>._, A<CancellationToken>._)).
                Returns(new CorrelaterResult<char>(10, "123\04".ToArray(), "5678\0".ToArray()));

            var match1 = new List<char>();
            var match2 = new List<char>();
            var totalDistance = 0L;
            var totalUpdates = 0;

            correlater.OnResultUpdate += result =>
            {
                match1.AddRange(result.BestMatch1);
                match2.AddRange(result.BestMatch2);
                totalDistance += result.Distance;
                totalUpdates++;
            };

            var actualResult = correlater.Correlate(string1, string2);
            var resultFromEvent = new CorrelaterResult<char>(totalDistance, match1.ToArray(), match2.ToArray());

            Assertions.AssertResultIsAsExpected(actualResult, resultFromEvent);
            Assert.AreEqual(timeToCallOrResultUpdate, totalUpdates, $"Got wrong number of {nameof(totalUpdates)}");
        }

        [TestMethod]
        [DataRow("abc1234def", "abc5678def", 2)] // Diff in the middle
        [DataRow("abc1234d", "abc5678d", 2)] 
        [DataRow("1234def", "5678def", 1)] // Diff at the beginning
        [DataRow("abc1234", "abc5678", 1)] // Diff at the end
        [DataRow("1234", "5678", 0)] // Diff eveywhere
        public void IContinuousCorrelaterTest_InnerCorrelaterIsContinuous(string string1, string string2, int extraTimesToCallOrResultUpdate)
        {
            var levenshteinCorrelater = new LevenshteinCorrelater<char>(missmatchCost, removalInsertionCost, removalInsertionCost);
            var splitToChunksCorrelater = new SplitToChunksCorrelaterWrapper<char>(levenshteinCorrelater, 2);
            var correlater = new IgnoreIdenticalBeginningAndEndCorrelaterWrapper<char>(splitToChunksCorrelater);

            var match1 = new List<char>();
            var match2 = new List<char>();
            var totalDistance = 0L;
            var totalUpdates = 0;
            var innerUpdates = 0;

            correlater.OnResultUpdate += result =>
            {
                match1.AddRange(result.BestMatch1);
                match2.AddRange(result.BestMatch2);
                totalDistance += result.Distance;
                totalUpdates++;
            };
            splitToChunksCorrelater.OnResultUpdate += _ => innerUpdates++;

            var actualResult = correlater.Correlate(string1, string2);
            var resultFromEvent = new CorrelaterResult<char>(totalDistance, match1.ToArray(), match2.ToArray());

            Assertions.AssertResultIsAsExpected(actualResult, resultFromEvent);
            Assert.AreEqual(extraTimesToCallOrResultUpdate + innerUpdates, totalUpdates, $"Got wrong number of {nameof(totalUpdates)}");
        }

        [TestMethod]
        public void CancellationToeknForInnerCorrelater()
        {
            var correlater = new IgnoreIdenticalBeginningAndEndCorrelaterWrapper<char>(new NeverEndingCorrelater<char>());
            correlater.AsseertCancellationTokenWorks();
        }

        [TestMethod]
        public void CancellationToeknForComparingStartElements()
        {
            var collection = SlowCompareElement.ToArrayOfSlowCompareElement("abcdefghijklmnop");
            var correlater = new IgnoreIdenticalBeginningAndEndCorrelaterWrapper<SlowCompareElement>(new NullCorrelator<SlowCompareElement>());
            correlater.AsseertCancellationTokenWorks(collection, collection);
        }

        [TestMethod]
        public void CancellationToeknForComparingEndElements()
        {
            var collection1 = SlowCompareElement.ToArrayOfSlowCompareElement("1abcdefghijklmnop");
            var collection2 = SlowCompareElement.ToArrayOfSlowCompareElement("2abcdefghijklmnop");
            var correlater = new IgnoreIdenticalBeginningAndEndCorrelaterWrapper<SlowCompareElement>(new NullCorrelator<SlowCompareElement>());
            correlater.AsseertCancellationTokenWorks(collection1, collection2);
        }
    }
}
