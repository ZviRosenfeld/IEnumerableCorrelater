using FakeItEasy;
using IEnumerableCorrelater.Correlaters;
using IEnumerableCorrelater.CorrelaterWrappers;
using IEnumerableCorrelater.Interfaces;
using IEnumerableCorrelater.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

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
            A.CallTo(() => innerCorrelater.Correlate(A<IEnumerable<char>>._, A<IEnumerable<char>>._)).Invokes((IEnumerable<char> collection1, IEnumerable<char> collection2) => 
            {
                Assert.IsTrue(!collection1.Any(), $"{nameof(collection1)} was not empty");
                Assert.IsTrue(!collection2.Any(), $"{nameof(collection2)} was not empty");
            });
            correlater.Correlate(s, s);
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
    }
}
