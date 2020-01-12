using FakeItEasy;
using IEnumerableCorrelater.Interfaces;
using IEnumerableCorrelater.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IEnumerableCorrelater.UnitTests
{
    [TestClass]
    public class StringCorrelaterTests
    {
        private static readonly ICorrelater<char> correlater = A.Fake<ICorrelater<char>>();
        private static readonly StringCorrelater stringCorrelater = new StringCorrelater(correlater);

        [TestMethod]
        public void Compare_CallsCorrelatorCompareMethodWithRightStrings()
        {
            var string1 = "abc";
            var string2 = "def";

            A.CallTo(() => correlater.Compare(A<ICollectionWrapper<char>>._, A<ICollectionWrapper<char>>._)).Invokes(
                (ICollectionWrapper<char> s1, ICollectionWrapper<char> s2) =>
                {
                    s1.AssertAreSame(string1.ToCharArray(), "Got wrong collectionWrapper");
                    s2.AssertAreSame(string2.ToCharArray(), "Got wrong collectionWrapper");
                });

            stringCorrelater.Correlate(string1, string2);
        }

        [TestMethod]
        public void Compare_GetCorrelatorCompareMethodResult()
        {
            var string1 = "abc";
            var string2 = "def";

            var expectedResult = A.Fake<CorrelaterResult<char>>();
            A.CallTo(() => correlater.Compare(A<ICollectionWrapper<char>>._, A<ICollectionWrapper<char>>._))
                .Returns(expectedResult);

            var result = stringCorrelater.Correlate(string1, string2);
            Assert.AreSame(expectedResult, result);
        }
    }
}
