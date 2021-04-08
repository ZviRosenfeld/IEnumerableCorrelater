using IEnumerableCorrelater.Correlaters;
using IEnumerableCorrelater.Exceptions;
using IEnumerableCorrelater.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IEnumerableCorrelater.UnitTests.Correlaters
{
    [TestClass]
    public class PatienceSortingAlgorithmTests
    {
        [TestMethod]
        [ExpectedException(typeof(EnumerableCorrelaterException))]
        public void CorrelateNonNullibleTypes_ThrowException() =>
            new PatienceDiffCorrelater<int>();

        [TestMethod]
        public void CorrelateNullibleTypes_DontThrowException() =>
            new PatienceDiffCorrelater<int?>();

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void CorrelateStrings_NullElementInCollection_ThrowException(bool nullInCollection1)
        {
            var collectionWithNull = new[] { "A", "B", "C", null };
            var collectionWithoutNull = new[] { "A", "B", "C" };

            var correlater = new PatienceDiffCorrelater<string>();
            if (nullInCollection1)
                correlater.AssetThrowsNullElementException(collectionWithNull, collectionWithoutNull, "collection1", 3);
            else
                correlater.AssetThrowsNullElementException(collectionWithoutNull, collectionWithNull, "collection2", 3);
        }

        [TestMethod]
        public void SameStringTest()
        {
            var s = "abcdefg";
            var correlater = new PatienceDiffCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(0, s.ToCharArray(), s.ToCharArray());

            correlater.AssertComparision(s, s, expectedResult);
        }

        [TestMethod]
        public void OneRemovedCharInStringsTest()
        {
            var s1 = "abcdefg";
            var s2 = "abcefg";
            var correlater = new PatienceDiffCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(1, s1.ToCharArray(), "abc\0efg".ToCharArray());

            correlater.AssertComparision(s1, s2, expectedResult);
        }

        [TestMethod]
        public void OneAddedCharInStringsTest()
        {
            var s1 = "abcefg";
            var s2 = "abcdefg";
            var correlater = new PatienceDiffCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(1, "abc\0efg".ToCharArray(), s2.ToCharArray());

            correlater.AssertComparision(s1, s2, expectedResult);
        }

        [TestMethod]
        public void TwoAddedCharsInStringsTest()
        {
            var s1 = "abcfg";
            var s2 = "abcdefg";
            var correlater = new PatienceDiffCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(2, "abc\0\0fg".ToCharArray(), s2.ToCharArray());

            correlater.AssertComparision(s1, s2, expectedResult);
        }

        [TestMethod]
        public void DifferentCharTest()
        {
            var s1 = "abc1efg";
            var s2 = "abc2efg";
            var correlater = new PatienceDiffCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(2, "abc1\0efg".ToCharArray(), "abc\02efg".ToCharArray());

            correlater.AssertComparision(s1, s2, expectedResult);
        }

        [TestMethod]
        public void TotallyDifferentStrings()
        {
            var s1 = "abcdefg";
            var s2 = "1234567";
            var correlater = new PatienceDiffCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(14, "abcdefg\0\0\0\0\0\0\0".ToCharArray(), "\0\0\0\0\0\0\01234567".ToCharArray());

            correlater.AssertComparision(s1, s2, expectedResult);
        }

        [TestMethod]
        public void EndTheSame()
        {
            var s1 = "1a";
            var s2 = "5a";
            var correlater = new PatienceDiffCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(2, "1\0a".ToCharArray(), "\05a".ToCharArray());

            correlater.AssertComparision(s1, s2, expectedResult);
        }

        [TestMethod]
        public void OneElementTheSame()
        {
            var s1 = "1a3";
            var s2 = "5a6";
            var correlater = new PatienceDiffCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(4, "1\0a3\0".ToCharArray(), "\05a\06".ToCharArray());

            correlater.AssertComparision(s1, s2, expectedResult);
        }

        [TestMethod]
        public void DifferentStart()
        {
            var s1 = "123abc";
            var s2 = "78abc";
            var correlater = new PatienceDiffCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(5, "123\0\0abc".ToCharArray(), "\0\0\078abc".ToCharArray());

            correlater.AssertComparision(s1, s2, expectedResult);
        }

        [TestMethod]
        public void SameStringInTheMiddle()
        {
            var s1 = "123abc456";
            var s2 = "78abc";
            var correlater = new PatienceDiffCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(8, "123\0\0abc456".ToCharArray(), "\0\0\078abc\0\0\0".ToCharArray());

            correlater.AssertComparision(s1, s2, expectedResult);
        }

        [TestMethod]
        public void ComplexString1()
        {
            var s1 = "1a9z888b2";
            var s2 = "2a7z6b4";
            var correlater = new PatienceDiffCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(10, "1\0a9\0z888\0b2\0".ToCharArray(), "\02a\07z\0\0\06b\04".ToCharArray());

            correlater.AssertComparision(s1, s2, expectedResult);
        }

        [TestMethod]
        public void ComplexString2()
        {
            var s1 = "1ya9az888b2";
            var s2 = "2ya7az6b4";
            var correlater = new PatienceDiffCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(10, "1\0ya9\0az888\0b2\0".ToCharArray(), "\02ya\07az\0\0\06b\04".ToCharArray());

            correlater.AssertComparision(s1, s2, expectedResult);
        }
    }
}
