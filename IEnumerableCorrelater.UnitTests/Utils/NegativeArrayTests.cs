using IEnumerableCorrelater.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IEnumerableCorrelater.UnitTests.Utils
{
    [TestClass]
    public class NegativeArrayTests
    {
        [TestMethod]
        [DataRow(0)]
        [DataRow(5)]
        [DataRow(9)]
        [DataRow(-1)]
        [DataRow(-10)]
        [DataRow(-5)]
        public void AccessWithIndex_GetRightElement(int index)
        {
            var array = new NegativeArray<int>(10);
            array[index] = 10;
            Assert.AreEqual(10, array[index]);
        }

        [TestMethod]
        [DataRow(-10, 0)]
        [DataRow(-1, 9)]
        [DataRow(5, -5)]
        public void MatchingNegativeAndPositiveIndexes(int negativeIndex, int positiveIndex)
        {
            var array = new NegativeArray<int>(10);
            array[negativeIndex] = 10;
            Assert.AreEqual(10, array[positiveIndex]);

            array[positiveIndex] = 10;
            Assert.AreEqual(10, array[negativeIndex]);
        }
    }
}
