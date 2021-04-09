using System.Linq;
using IEnumerableCorrelater.Correlaters;
using IEnumerableCorrelater.CorrelaterWrappers;
using IEnumerableCorrelater.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IEnumerableCorrelater.UnitTests.CorrelaterWrappers
{
    [TestClass]
    public class SplitByPatienceAlgorithmWrapperTests
    {
        [TestMethod]
        public void SameCollection_NeverCallInnerCorrelater()
        {
            var innerCorrelater = new SaveElementsInnerCorrelater<char>();
            var wrapper = new SplitByPatienceAlgorithmWrapper<char>(innerCorrelater);

            var s = "123456789";

            wrapper.Correlate(s, s);

            Assert.IsFalse(innerCorrelater.CorrelatedCollections.Any(c => c.Item1.Any() || c.Item2.Any()));
        }

        [TestMethod]
        public void OneSameElement_SplitByIt()
        {
            var innerCorrelater = new SaveElementsInnerCorrelater<char>();
            var wrapper = new SplitByPatienceAlgorithmWrapper<char>(innerCorrelater, false);

            var s1 = "123456789";
            var s2 = "abcd5fghi";

            wrapper.Correlate(s1, s2);

            Assert.AreEqual(2, innerCorrelater.CorrelatedCollections.Count);
            CollectionAssert.AreEqual("1234".ToCharArray(), innerCorrelater.CorrelatedCollections[0].Item1);
            CollectionAssert.AreEqual("abcd".ToCharArray(), innerCorrelater.CorrelatedCollections[0].Item2);
            CollectionAssert.AreEqual("6789".ToCharArray(), innerCorrelater.CorrelatedCollections[1].Item1);
            CollectionAssert.AreEqual("fghi".ToCharArray(), innerCorrelater.CorrelatedCollections[1].Item2);
        }

        [TestMethod]
        public void OneSameElement_SplitByIt_2()
        {
            var innerCorrelater = new SaveElementsInnerCorrelater<char>();
            var wrapper = new SplitByPatienceAlgorithmWrapper<char>(innerCorrelater, false);

            var s1 = "123456789";
            var s2 = "5fghi";

            wrapper.Correlate(s1, s2);

            Assert.AreEqual(2, innerCorrelater.CorrelatedCollections.Count);
            CollectionAssert.AreEqual("1234".ToCharArray(), innerCorrelater.CorrelatedCollections[0].Item1);
            CollectionAssert.AreEqual("".ToCharArray(), innerCorrelater.CorrelatedCollections[0].Item2);
            CollectionAssert.AreEqual("6789".ToCharArray(), innerCorrelater.CorrelatedCollections[1].Item1);
            CollectionAssert.AreEqual("fghi".ToCharArray(), innerCorrelater.CorrelatedCollections[1].Item2);
        }

        [TestMethod]
        public void OneSameElement_SplitByIt_3()
        {
            var innerCorrelater = new SaveElementsInnerCorrelater<char>();
            var wrapper = new SplitByPatienceAlgorithmWrapper<char>(innerCorrelater, false);

            var s1 = "123456789";
            var s2 = "abcd5";

            wrapper.Correlate(s1, s2);

            Assert.AreEqual(2, innerCorrelater.CorrelatedCollections.Count);
            CollectionAssert.AreEqual("1234".ToCharArray(), innerCorrelater.CorrelatedCollections[0].Item1);
            CollectionAssert.AreEqual("abcd".ToCharArray(), innerCorrelater.CorrelatedCollections[0].Item2);
            CollectionAssert.AreEqual("6789".ToCharArray(), innerCorrelater.CorrelatedCollections[1].Item1);
            CollectionAssert.AreEqual("".ToCharArray(), innerCorrelater.CorrelatedCollections[1].Item2);
        }

        [TestMethod]
        public void TwoSameElements_SplitByThem()
        {
            var innerCorrelater = new SaveElementsInnerCorrelater<char>();
            var wrapper = new SplitByPatienceAlgorithmWrapper<char>(innerCorrelater, false);

            var s1 = "123456789";
            var s2 = "abcd56ghi";

            wrapper.Correlate(s1, s2);

            Assert.AreEqual(3, innerCorrelater.CorrelatedCollections.Count);
            CollectionAssert.AreEqual("1234".ToCharArray(), innerCorrelater.CorrelatedCollections[0].Item1);
            CollectionAssert.AreEqual("abcd".ToCharArray(), innerCorrelater.CorrelatedCollections[0].Item2);
            CollectionAssert.AreEqual("".ToCharArray(), innerCorrelater.CorrelatedCollections[1].Item1);
            CollectionAssert.AreEqual("".ToCharArray(), innerCorrelater.CorrelatedCollections[1].Item2);
            CollectionAssert.AreEqual("789".ToCharArray(), innerCorrelater.CorrelatedCollections[2].Item1);
            CollectionAssert.AreEqual("ghi".ToCharArray(), innerCorrelater.CorrelatedCollections[2].Item2);
        }

        [TestMethod]
        public void TwoSameElements_SplitByThem_2()
        {
            var innerCorrelater = new SaveElementsInnerCorrelater<char>();
            var wrapper = new SplitByPatienceAlgorithmWrapper<char>(innerCorrelater, false);

            var s1 = "123456789";
            var s2 = "a2cde7g";

            wrapper.Correlate(s1, s2);

            Assert.AreEqual(3, innerCorrelater.CorrelatedCollections.Count);
            CollectionAssert.AreEqual("1".ToCharArray(), innerCorrelater.CorrelatedCollections[0].Item1);
            CollectionAssert.AreEqual("a".ToCharArray(), innerCorrelater.CorrelatedCollections[0].Item2);
            CollectionAssert.AreEqual("3456".ToCharArray(), innerCorrelater.CorrelatedCollections[1].Item1);
            CollectionAssert.AreEqual("cde".ToCharArray(), innerCorrelater.CorrelatedCollections[1].Item2);
            CollectionAssert.AreEqual("89".ToCharArray(), innerCorrelater.CorrelatedCollections[2].Item1);
            CollectionAssert.AreEqual("g".ToCharArray(), innerCorrelater.CorrelatedCollections[2].Item2);
        }

        [TestMethod]
        public void OnProgressUpdatesHappensRightNumberOfTimes()
        {
            var s1 = "123456789";
            var s2 = "a2cdef7gh";

            var correlater = new SplitByPatienceAlgorithmWrapper<char>(new NullCorrelator<char>());
            correlater.AssertProgressUpdateWasCalledRightNumberOfTimes(s1.ToCharArray(), s2.ToCharArray(), 5);
        }

    }
}
