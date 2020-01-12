using System.Collections.Generic;
using FakeItEasy;
using IEnumerableCorrelater.Interfaces;
using IEnumerableCorrelater.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IEnumerableCorrelater.UnitTests
{
    [TestClass]
    public class EnumerableCorrelaterTests
    {
        private static readonly ICorrelater<string> correlater = A.Fake<ICorrelater<string>>();
        private static readonly EnumerableCorrelater<string> stringCorrelater = new EnumerableCorrelater<string>(correlater);

        [TestMethod]
        public void CompareArrays_CallsCorrelatorCompareMethodWithRightStrings()
        {
            var array1 = new[] { "A", "B", "C" };
            var array2 = new[] { "D", "E", "F" };

            A.CallTo(() => correlater.Compare(A<ICollectionWrapper<string>>._, A<ICollectionWrapper<string>>._)).Invokes(
                (ICollectionWrapper<string> s1, ICollectionWrapper<string> s2) =>
                {
                    s1.AssertAreSame(array1, "Got wrong collectionWrapper");
                    s2.AssertAreSame(array2, "Got wrong collectionWrapper");
                });

            stringCorrelater.Correlate(array1, array2);
        }

        [TestMethod]
        public void CompareLists_CallsCorrelatorCompareMethodWithRightStrings()
        {
            var array1 = new List<string> { "A", "B", "C" };
            var array2 = new List<string> { "D", "E", "F" };

            A.CallTo(() => correlater.Compare(A<ICollectionWrapper<string>>._, A<ICollectionWrapper<string>>._)).Invokes(
                (ICollectionWrapper<string> s1, ICollectionWrapper<string> s2) =>
                {
                    s1.AssertAreSame(array1, "Got wrong collectionWrapper");
                    s2.AssertAreSame(array2, "Got wrong collectionWrapper");
                });

            stringCorrelater.Correlate(array1, array2);
        }

        [TestMethod]
        public void Compare_GetCorrelatorCompareMethodResult()
        {
            var array1 = new[] { "A", "B", "C" };
            var array2 = new[] { "D", "E", "F" };

            var expectedResult = A.Fake<CorrelaterResult<string>>();
            A.CallTo(() => correlater.Compare(A<ICollectionWrapper<string>>._, A<ICollectionWrapper<string>>._))
                .Returns(expectedResult);

            var result = stringCorrelater.Correlate(array1, array2);
            Assert.AreSame(expectedResult, result);
        }
    }
}
