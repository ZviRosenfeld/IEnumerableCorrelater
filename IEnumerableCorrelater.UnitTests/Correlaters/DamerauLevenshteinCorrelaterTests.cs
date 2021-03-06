﻿using FakeItEasy;
using IEnumerableCorrelater.Calculators;
using IEnumerableCorrelater.Correlaters;
using IEnumerableCorrelater.Exceptions;
using IEnumerableCorrelater.Interfaces;
using IEnumerableCorrelater.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IEnumerableCorrelater.UnitTests.Correlaters
{
    [TestClass]
    public class DamerauLevenshteinCorrelaterTests
    {
        private const int removalCost = 9;
        private const int insertionCost = 10;
        private const int missmatchCost = 11;
        private const int transpositionCost = 15;
        private static readonly DamerauLevenshteinCorrelater<string> correlater = new DamerauLevenshteinCorrelater<string>(missmatchCost, transpositionCost, removalCost, insertionCost);
        private static readonly DamerauLevenshteinCorrelater<char> stringCorrelater = new DamerauLevenshteinCorrelater<char>(missmatchCost, transpositionCost, removalCost, insertionCost);

        [TestMethod]
        [ExpectedException(typeof(EnumerableCorrelaterException))]
        public void CorrelateNonNullibleTypes_ThrowException() =>
            new LevenshteinCorrelater<int>(null, removalCost, insertionCost);

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void CorrelateStrings_NullElementInCollection_ThrowException(bool nullInCollection1)
        {
            var collectionWithNull = new[] {"A", "B", "C", null};
            var collectionWithoutNull = new[] { "A", "B", "C" };

            if (nullInCollection1)
                correlater.AssetThrowsNullElementException(collectionWithNull, collectionWithoutNull, "collection1", 3);
            else
                correlater.AssetThrowsNullElementException(collectionWithoutNull, collectionWithNull, "collection2", 3);
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void CorrelateChar_NullElementInCollection_ThrowException(bool nullInCollection1)
        {
            var collectionWithNull = new[] { 'a', 'b', 'c', '\0' };
            var collectionWithoutNull = new[] { 'a', 'b', 'c' };

            if (nullInCollection1)
                stringCorrelater.AssetThrowsNullElementException(collectionWithNull, collectionWithoutNull, "collection1", 3);
            else
                stringCorrelater.AssetThrowsNullElementException(collectionWithoutNull, collectionWithNull, "collection2", 3);
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void CorrelateObject_NullElementInCollection_ThrowException(bool nullInCollection1)
        {
            var collectionWithNull = new[] { new TestClass(), null };
            var collectionWithoutNull = new[] { new TestClass() };

            var correlater = new DamerauLevenshteinCorrelater<TestClass>(missmatchCost, transpositionCost, removalCost, insertionCost);
            if (nullInCollection1)
                correlater.AssetThrowsNullElementException(collectionWithNull, collectionWithoutNull, "collection1", 1);
            else
                correlater.AssetThrowsNullElementException(collectionWithoutNull, collectionWithNull, "collection2", 1);
        }

        [TestMethod]
        public void CorrelateNullibleTypes_DontThrowException() =>
            new LevenshteinCorrelater<int?>(null, removalCost, insertionCost);

        [TestMethod]
        public void CorrelateChar_DontThrowException() =>
            new LevenshteinCorrelater<char>(null, removalCost, insertionCost);

        [TestMethod]
        public void CorrelateEmptyArrayToFullArray()
        {
            var array1 = new string[0];
            var array2 = new[] { "A", "B", "C" };

            var expectedResult = new CorrelaterResult<string>(insertionCost * array2.Length, new string[3], array2);
            correlater.AssertComparision(array1, array2, expectedResult);
        }

        [TestMethod]
        public void CorrelateFullArrayToEmptyArray()
        {
            var array1 = new[] { "A", "B", "C" };
            var array2 = new string[0];

            var expectedResult = new CorrelaterResult<string>(removalCost * array1.Length, array1, new string[3]);
            correlater.AssertComparision(array1, array2, expectedResult);
        }

        [TestMethod]
        public void Correlate_OneElementToInsert()
        {
            var array1 = new[] { "A", "C" };
            var array2 = new[] { "A", "B", "C" };

            var expectedResult = new CorrelaterResult<string>(insertionCost, new[] { "A", null, "C" }, array2);
            correlater.AssertComparision(array1, array2, expectedResult);
        }

        [TestMethod]
        public void Correlate_OneElementToRemove()
        {
            var array1 = new[] { "A", "B", "C" };
            var array2 = new[] { "A", "C" };

            var expectedResult = new CorrelaterResult<string>(removalCost, array1, new[] { "A", null, "C" });
            correlater.AssertComparision(array1, array2, expectedResult);
        }

        [TestMethod]
        public void Correlate_Substitution()
        {
            var array1 = new[] { "A", "D", "C" };
            var array2 = new[] { "A", "B", "C" };

            var expectedResult = new CorrelaterResult<string>(missmatchCost, array1, array2);
            correlater.AssertComparision(array1, array2, expectedResult);
        }

        [TestMethod]
        public void Correlate_Transposition1()
        {
            var array1 = new[] {"A", "B"};
            var array2 = new[] { "B", "A"};

            var expectedResult = new CorrelaterResult<string>(transpositionCost, array1, array2);
            correlater.AssertComparision(array1, array2, expectedResult);
        }

        [TestMethod]
        public void Correlate_Transposition2()
        {
            var array1 = new[] { "A", "D", "B", "C" };
            var array2 = new[] { "A", "B", "D", "C" };

            var expectedResult = new CorrelaterResult<string>(transpositionCost, array1, array2);
            correlater.AssertComparision(array1, array2, expectedResult);
        }

        [TestMethod]
        public void Correlate_Transposition3()
        {
            var correlater = new DamerauLevenshteinCorrelater<string>(20, 10, 15, 15);

            var array1 = new[] { "A", "D", "B" };
            var array2 = new[] { "B", "D" };

            var expectedResult = new CorrelaterResult<string>(25, array1, new []{null, "B", "D"});
            correlater.AssertComparision(array1, array2, expectedResult);
        }

        [TestMethod]
        public void Correlate_RemoveAndReplace()
        {
            var array1 = new[] { "C", "A", "B" };
            var array2 = new[] { "B", "A" };

            var expectedResult = new CorrelaterResult<string>(missmatchCost + removalCost, array1, new []{"B", "A", null});
            correlater.AssertComparision(array1, array2, expectedResult);
        }


        [TestMethod]
        public void CorrelateString()
        {
            var string1 = "abc";
            var string2 = "ac";

            var expectedResult = new CorrelaterResult<char>(removalCost, string1.ToCharArray(), "a\0c".ToCharArray());
            stringCorrelater.AssertComparision(string1, string2, expectedResult);
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(10)]
        public void DifrentTranspositionCosts(int transpositionCost)
        {
            var transpositionCalculator = A.Fake<ITranspositionCalculator<string>>();
            var correlater = new DamerauLevenshteinCorrelater<string>(new BasicDistanceCalculator<string>(20),  transpositionCalculator, 20, 20);
            A.CallTo(() => transpositionCalculator.TranspositionCost(A<string>._, A<string>._)).Returns((uint)transpositionCost);

            var array1 = new[] { "V", "E" };
            var array2 = new[] { "E", "V" };

            var expectedResult = new CorrelaterResult<string>(transpositionCost, array1, array2);
            correlater.AssertComparision(array1, array2, expectedResult);
        }

        [TestMethod]
        public void SecondStringEmptyTest()
        {
            var s = "12345678";
            var expectedResult = new CorrelaterResult<char>(s.Length * removalCost, s.ToCharArray(), "\0\0\0\0\0\0\0\0".ToCharArray());

            stringCorrelater.AssertComparision(s, string.Empty, expectedResult);
        }

        [TestMethod]
        public void FirstStringEmptyTest()
        {
            var s = "12345678";
            var expectedResult = new CorrelaterResult<char>(s.Length * insertionCost, "\0\0\0\0\0\0\0\0".ToCharArray(), s.ToCharArray());

            stringCorrelater.AssertComparision(string.Empty, s, expectedResult);
        }

        [TestMethod]
        public void OnProgressUpdatesHappensRightNumberOfTimes()
        {
            var correlater = new DamerauLevenshteinCorrelater<string>(missmatchCost, transpositionCost, removalCost, insertionCost);
            correlater.AssertProgressUpdateWasCalledRightNumberOfTimes();
        }

        [TestMethod]
        public void CancellationToeknWorks()
        {
            stringCorrelater.AsseertCancellationTokenWorks();
        }
    }
}
