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
        public void OneDifferentCharTest()
        {
            var s1 = "abcdefg";
            var s2 = "abc1efg";

            var correlater = new HuntSzymanskiCorrelater<char>();
            var expectedResult = new CorrelaterResult<char>(0, "abc\0defg".ToCharArray(), "abc1\0efg".ToCharArray());

            correlater.AssertComparision(s1, s2, expectedResult);
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
