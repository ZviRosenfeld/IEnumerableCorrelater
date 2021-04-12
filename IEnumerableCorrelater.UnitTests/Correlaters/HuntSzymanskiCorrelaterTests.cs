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
    }
}
