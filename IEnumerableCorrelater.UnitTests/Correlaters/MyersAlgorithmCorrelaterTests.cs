using IEnumerableCorrelater.Correlaters;
using IEnumerableCorrelater.Exceptions;
using IEnumerableCorrelater.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IEnumerableCorrelater.UnitTests.Correlaters
{
    [TestClass]
    public class MyersAlgorithmCorrelaterTests
    {
        [TestMethod]
        [ExpectedException(typeof(EnumerableCorrelaterException))]
        public void CorrelateNonNullibleTypes_ThrowException() =>
            new MyersAlgorithmCorrelater<int>();

        [TestMethod]
        public void CorrelateNullibleTypes_DontThrowException() =>
            new MyersAlgorithmCorrelater<int?>();

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void CorrelateStrings_NullElementInCollection_ThrowException(bool nullInCollection1)
        {
            var collectionWithNull = new[] { "A", "B", "C", null };
            var collectionWithoutNull = new[] { "A", "B", "C" };

            var correlater = new MyersAlgorithmCorrelater<string>();
            if (nullInCollection1)
                correlater.AssetThrowsNullElementException(collectionWithNull, collectionWithoutNull, "collection1", 3);
            else
                correlater.AssetThrowsNullElementException(collectionWithoutNull, collectionWithNull, "collection2", 3);
        }

        [TestMethod]
        public void SameStringTest()
        {
            var s = "abcdefg";
            var correlater = new MyersAlgorithmCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(0, s.ToCharArray(), s.ToCharArray());

            correlater.AssertComparision(s, s, expectedResult);
        }

        [TestMethod]
        public void OneRemovedCharInStringsTest()
        {
            var s1 = "abcdefg";
            var s2 = "abcefg";
            var correlater = new MyersAlgorithmCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(1, s1.ToCharArray(), "abc\0efg".ToCharArray());

            correlater.AssertComparision(s1, s2, expectedResult);
        }

        [TestMethod]
        public void OneAddedCharInStringsTest()
        {
            var s1 = "abcefg";
            var s2 = "abcdefg";
            var correlater = new MyersAlgorithmCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(1, "abc\0efg".ToCharArray(), s2.ToCharArray());

            correlater.AssertComparision(s1, s2, expectedResult);
        }

        [TestMethod]
        public void OneDifferentCharInStringsTest()
        {
            var s1 = "abcdefg";
            var s2 = "abc1efg";
            var correlater = new MyersAlgorithmCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(2, "abcd\0efg".ToCharArray(), "abc\01efg".ToCharArray());

            correlater.AssertComparision(s1, s2, expectedResult);
        }

        [TestMethod]
        public void CharRemovedAndAddedTest()
        {
            var s1 = "abdefg";
            var s2 = "abcdef";
            var correlater = new MyersAlgorithmCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(2, "ab\0defg".ToCharArray(), "abcdef\0".ToCharArray());

            correlater.AssertComparision(s1, s2, expectedResult);
        }

        [TestMethod]
        public void SomeCharsRemovedTest()
        {
            var s1 = "abcdefghijk";
            var s2 = "acdeghk";
            var correlater = new MyersAlgorithmCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(4, s1.ToCharArray(), "a\0cde\0gh\0\0k".ToCharArray());

            correlater.AssertComparision(s1, s2, expectedResult);
        }

        [TestMethod]
        public void TotallyDifferentStringsTest()
        {
            var s1 = "abcdefghij";
            var s2 = "12345678";
            var correlater = new MyersAlgorithmCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(s1.Length + s2.Length, "abcdefghij\0\0\0\0\0\0\0\0".ToCharArray(), "\0\0\0\0\0\0\0\0\0\012345678".ToCharArray());

            correlater.AssertComparision(s1, s2, expectedResult);
        }

        [TestMethod]
        public void CancellationToeknWorks()
        {
            var correlater = new MyersAlgorithmCorrelater<char>();
            correlater.AsseertCancellationTokenWorks();
        }
    }
}
