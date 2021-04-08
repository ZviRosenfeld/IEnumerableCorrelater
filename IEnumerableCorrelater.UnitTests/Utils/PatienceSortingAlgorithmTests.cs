using IEnumerableCorrelater.UnitTests.TestUtils;
using IEnumerableCorrelater.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace IEnumerableCorrelater.UnitTests.Utils
{
    [TestClass]
    public class PatienceSortingAlgorithmTests
    {
        [TestMethod]
        public void GetRisingIndexes_EmptyArray_ReturnEmptyIndexes()
        {
            var input = new int[0];
            var expected = new int[0];
            var patienceSortingAlgorithm = new PatienceSortingAlgorithm();
            var actaul = patienceSortingAlgorithm.GetRisingIndexes(input);

            CollectionAssert.AreEqual(expected, actaul);
        }

        [TestMethod]
        public void GetRisingIndexes_OrderedArray_ReturnAllIndexes()
        {
            var input = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            var patienceSortingAlgorithm = new PatienceSortingAlgorithm();
            var actaul = patienceSortingAlgorithm.GetRisingIndexes(input);

            CollectionAssert.AreEqual(input.ToArray(), actaul);
        }

        [TestMethod]
        public void GetRisingIndexes_ScatteredArray_ReturnRightIndexes()
        {
            var input = new int[] { 1, 8, 2, 3, 10, 4, 5, 9, 6, 7 };
            var expected = new int[] { 1, 2, 3, 4, 5, 6, 7 };
            var patienceSortingAlgorithm = new PatienceSortingAlgorithm();
            var actaul = patienceSortingAlgorithm.GetRisingIndexes(input);

            CollectionAssert.AreEqual(expected, actaul);
        }
    }
}
