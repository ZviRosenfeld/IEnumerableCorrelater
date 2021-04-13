using IEnumerableCorrelater.Correlaters;
using IEnumerableCorrelater.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IEnumerableCorrelater.UnitTests.Correlaters
{
    [TestClass]
    public class HuntSzymanskiCorrelaterTests
    {
        [TestMethod]
        public void SameStringTest()
        {
            var s = "abcdefg";
            var correlater = new HuntSzymanskiCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(0, s.ToCharArray(), s.ToCharArray());

            correlater.AssertComparision(s, s, expectedResult);
        }

        [TestMethod]
        public void OneMissingCharTest()
        {
            var s1 = "abcefg";
            var s2 = "abc1efg";

            var correlater = new HuntSzymanskiCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(1, "abc\0efg".ToCharArray(), "abc1efg".ToCharArray());

            correlater.AssertComparision(s1, s2, expectedResult);
        }

        [TestMethod]
        public void OneAddedCharTest()
        {
            var s1 = "abc1efg";
            var s2 = "abcefg";

            var correlater = new HuntSzymanskiCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(1, "abc1efg".ToCharArray(), "abc\0efg".ToCharArray());

            correlater.AssertComparision(s1, s2, expectedResult);
        }

        [TestMethod]
        public void OneDifferentCharTest()
        {
            var s1 = "abc1efg";
            var s2 = "abc2efg";

            var correlater = new HuntSzymanskiCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(2, "abc\01efg".ToCharArray(), "abc2\0efg".ToCharArray());

            correlater.AssertComparision(s1, s2, expectedResult);
        }

        [TestMethod]
        public void DifferentStartCharTest()
        {
            var s1 = "12ab";
            var s2 = "34ab";

            var correlater = new HuntSzymanskiCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(4, "\0\012ab".ToCharArray(), "34\0\0ab".ToCharArray());

            correlater.AssertComparision(s1, s2, expectedResult);
        }

        [TestMethod]
        public void DoubleCharTest()
        {
            var s = "22ab";

            var correlater = new HuntSzymanskiCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(0, s.ToCharArray(), s.ToCharArray());

            correlater.AssertComparision(s, s, expectedResult);
        }

        [TestMethod]
        public void DoubleCharTest2()
        {
            var s = "123421";

            var correlater = new HuntSzymanskiCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(0, s.ToCharArray(), s.ToCharArray());

            correlater.AssertComparision(s, s, expectedResult);
        }

        [TestMethod]
        public void TotallyDifferentStringsTest()
        {
            var s1 = "abcdefghij";
            var s2 = "12345678";
            var correlater = new HuntSzymanskiCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(s1.Length + s2.Length, "\0\0\0\0\0\0\0\0abcdefghij".ToCharArray(), "12345678\0\0\0\0\0\0\0\0\0\0".ToCharArray());

            correlater.AssertComparision(s1, s2, expectedResult);
        }

        [TestMethod]
        public void SecondStringEmptyTest()
        {
            var s = "12345678";
            var correlater = new HuntSzymanskiCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(s.Length, s.ToCharArray(), "\0\0\0\0\0\0\0\0".ToCharArray());

            correlater.AssertComparision(s, string.Empty, expectedResult);
        }

        [TestMethod]
        public void FirstStringEmptyTest()
        {
            var s = "12345678";
            var correlater = new HuntSzymanskiCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(s.Length, "\0\0\0\0\0\0\0\0".ToCharArray(), s.ToCharArray());

            correlater.AssertComparision(string.Empty, s, expectedResult);
        }

        [TestMethod]
        public void CancellationToeknWorks()
        {
            var correlater = new HuntSzymanskiCorrelater<char>();
            correlater.AsseertCancellationTokenWorks();
        }

        [TestMethod]
        public void OnProgressUpdatesHappensRightNumberOfTimes()
        {
            var s1 = "abcdefghij";
            var s2 = "abcd6fghij";

            var correlater = new HuntSzymanskiCorrelater<char>();
            correlater.AssertProgressUpdateWasCalledRightNumberOfTimes(s1.ToCharArray(), s2.ToCharArray(), 10);
        }
    }
}
