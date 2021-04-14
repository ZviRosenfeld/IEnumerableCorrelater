using IEnumerableCorrelater.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace IEnumerableCorrelater.UnitTests.Utils
{
    [TestClass]
    public class UtilsTests
    {
        [TestMethod]
        public void GetLcsFromResult_CollectionTheSame_FullCollection()
        {
            var collection = "1234567890".ToCharArray();
            AssertWeGetTheRightLcs(collection, collection, collection);
        }

        [TestMethod]
        public void GetLcsFromResult_OneMissingChar_FullCollection()
        {
            var collection1 = "1234567890".ToCharArray();
            var collection2 = "12345\07890".ToCharArray();
            AssertWeGetTheRightLcs(collection1, collection2, collection2.Where(c => c != default).ToArray());
        }

        [TestMethod]
        public void GetLcsFromResult_OneAddedChar_FullCollection()
        {
            var collection1 = "12345\07890".ToCharArray();
            var collection2 = "1234567890".ToCharArray();
            AssertWeGetTheRightLcs(collection1, collection2, collection1.Where(c => c != default).ToArray());
        }

        [TestMethod]
        public void GetLcsFromResult_OnReplacedChar_FullCollection()
        {
            var collection1 = "1234567890".ToCharArray();
            var collection2 = "12345a7890".ToCharArray();
            AssertWeGetTheRightLcs(collection1, collection2, "123457890".ToCharArray());
        }

        [TestMethod]
        public void GetLcsFromResult_MissingLastElement()
        {
            var collection1 = "123456789\0".ToCharArray();
            var collection2 = "1234567890".ToCharArray();
            AssertWeGetTheRightLcs(collection1, collection2, "123456789".ToCharArray());
        }

        [TestMethod]
        public void GetLcsFromResult_AddedLastElement()
        {
            var collection1 = "1234567890".ToCharArray();
            var collection2 = "123456789\0".ToCharArray();
            AssertWeGetTheRightLcs(collection1, collection2, "123456789".ToCharArray());
        }

        [TestMethod]
        public void GetLcsFromResult_ManyDifferentElements()
        {
            var collection1 = "1\0\04567890".ToCharArray();
            var collection2 = "12345a78\0\0".ToCharArray();
            AssertWeGetTheRightLcs(collection1, collection2, "14578".ToCharArray());
        }

        [TestMethod]
        public void GetLcsFromResult_ManyDifferentElements2()
        {
            var collection1 = "\0\0c\0eefggg111kaalmn".ToCharArray();
            var collection2 = "abcd1efggghijkaa\0\0n".ToCharArray();
            AssertWeGetTheRightLcs(collection1, collection2, "cefgggkaan".ToCharArray());
        }

        [TestMethod]
        public void GetLcsFromResult_EmptyFirstCollection_EmptyLcs()
        {
            var collection2 = "1234567890".ToCharArray();
            var collection1 = collection2.Length.GetNNullElemenets<char>().ToArray();
            AssertWeGetTheRightLcs(collection1, collection2, "".ToCharArray());
        }

        [TestMethod]
        public void GetLcsFromResult_EmptySecondCollection_EmptyLcs()
        {
            var collection1 = "1234567890".ToCharArray();
            var collection2 = collection1.Length.GetNNullElemenets<char>().ToArray();
            AssertWeGetTheRightLcs(collection1, collection2, "".ToCharArray());
        }

        private void AssertWeGetTheRightLcs<T>(T[] collection1, T[] collection2, T[] expectedLcs)
        {
            var result = new CorrelaterResult<T>(0, collection1, collection2);
            var lcs = result.GetLcsFromResult();

            CollectionAssert.AreEqual(expectedLcs, lcs.ToList());
        }
    }
}
