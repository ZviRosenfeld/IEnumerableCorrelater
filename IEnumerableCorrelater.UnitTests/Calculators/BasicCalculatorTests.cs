using IEnumerableCorrelater.Calculators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IEnumerableCorrelater.UnitTests.Calculators
{
    /// <summary>
    /// This class tests that all the basic calculator throw exceptions if the cost is not greater than 0
    /// </summary>
    [TestClass]
    public class BasicCalculatorTests
    {
        [TestMethod]
        [DataRow(-1)]
        [ExpectedException(typeof(ArgumentException), "Transposition cost")]
        public void BasicTranspositionCalculator_BadCost_ThrowException(int cost) =>
            new BasicTranspositionCalculator<char>(cost);

        [TestMethod]
        [DataRow(-1)]
        [ExpectedException(typeof(ArgumentException), "Distance cost")]
        public void BasicDistanceCalculator_BadCost_ThrowException(int cost) =>
            new BasicDistanceCalculator<char>(cost);

        [TestMethod]
        [DataRow(-1)]
        [ExpectedException(typeof(ArgumentException), "Insertion cost")]
        public void BasicInsertionCalculator_BadCost_ThrowException(int cost) =>
            new BasicInsertionCalculator<char>(cost);

        [TestMethod]
        [DataRow(-1)]
        [ExpectedException(typeof(ArgumentException), "Removal cost")]
        public void BasicRemovalCalculator_BadCost_ThrowException(int cost) =>
            new BasicRemovalCalculator<char>(cost);
    }
}
