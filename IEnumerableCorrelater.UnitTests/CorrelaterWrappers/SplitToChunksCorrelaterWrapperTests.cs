﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using FakeItEasy;
using IEnumerableCorrelater.Correlaters;
using IEnumerableCorrelater.CorrelaterWrappers;
using IEnumerableCorrelater.Interfaces;
using IEnumerableCorrelater.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IEnumerableCorrelater.UnitTests.CorrelaterWrappers
{
    [TestClass]
    public class SplitToChunksCorrelaterWrapperTests
    {
        private const int removalInsertionCost = 7;
        private const int missmatchCost = 10;
        private const int CHUNK_SIZE = 6;
        private const string LONG_STRING = "abcdefghijklmnoqrstuvwxyz123456789";
        private static readonly int CHUNKS = LONG_STRING.Length / CHUNK_SIZE + (LONG_STRING.Length % CHUNK_SIZE == 0 ? 0 : 1);
        private static readonly ICorrelater<char> innerCorrelater = A.Fake<ICorrelater<char>>();
        private static readonly SplitToChunksCorrelaterWrapper<char> correlater = 
            new SplitToChunksCorrelaterWrapper<char>(innerCorrelater, CHUNK_SIZE);
        private static readonly SplitToChunksCorrelaterWrapper<char> levenshteinCorrelater = 
            new SplitToChunksCorrelaterWrapper<char>(new LevenshteinCorrelater<char>(missmatchCost, removalInsertionCost, removalInsertionCost), CHUNK_SIZE);

        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(6)]
        public void Correlate_ReturnsRightDistance(int distancePerChunk)
        {
            innerCorrelater.SetToRetrunInputCollection(distancePerChunk);

            var result = correlater.Compare(LONG_STRING.ToCollectionWrapper(), LONG_STRING.ToCollectionWrapper());
            Assert.AreEqual(distancePerChunk * CHUNKS, result.Distance);
        }

        [TestMethod]
        public void Correlate_NoOffset_DontMissPartOfMatch()
        {
            innerCorrelater.SetToRetrunInputCollection(0);

            var expectedResult = new CorrelaterResult<char>(0, LONG_STRING.ToCharArray(), LONG_STRING.ToCharArray());
            correlater.AssertComparision(LONG_STRING, LONG_STRING, expectedResult);
        }

        [TestMethod]
        public void Correlate_MatchWithOffsetForFirst_DontMissPartOfMatch()
        {
            innerCorrelater.SetToRetrun(AddOffset, AddNullToEnd);

            var result = correlater.Compare(LONG_STRING.ToCollectionWrapper(), LONG_STRING.ToCollectionWrapper());
            result.BestMatch1.RemoveNull().ToCollectionWrapper().AssertAreSame(result.BestMatch2.RemoveNull(), "Collection is missing elements");
        }

        [TestMethod]
        public void Correlate_MatchWithOffsetForSecond_DontMissPartOfMatch()
        {
            innerCorrelater.SetToRetrun(AddNullToEnd, AddOffset);

            var result = correlater.Compare(LONG_STRING.ToCollectionWrapper(), LONG_STRING.ToCollectionWrapper());
            result.BestMatch1.RemoveNull().ToCollectionWrapper().AssertAreSame(result.BestMatch2.RemoveNull(), "Collection is missing elements");
        }

        [TestMethod]
        public void Correlate_StringsOfDiffrentLength_String1Longer()
        {
            var string1 = "AB";
            var string2 = "A";
            innerCorrelater.SetToRetrunInputCollection(0);

            var expectedResult = new CorrelaterResult<char>(-1, string1.ToCharArray(), new[] { 'A', '\0' });
            levenshteinCorrelater.AssertComparision(string1, string2, expectedResult);
        }

        [TestMethod]
        public void Correlate_StringsOfDiffrentLength_String2Longer()
        {
            var string1 = "A";
            var string2 = "AB";
            innerCorrelater.SetToRetrunInputCollection(0);

            var expectedResult = new CorrelaterResult<char>(-1, new[] { 'A', '\0' }, string2.ToCharArray());
            levenshteinCorrelater.AssertComparision(string1, string2, expectedResult);
        }

        [TestMethod]
        public void Correlate_Strings1MuchLongerThanString2()
        {
            innerCorrelater.SetToRetrunInputCollection(0);

            var expectedResult = new CorrelaterResult<char>(-1, LONG_STRING.ToCharArray(), AddNullToEnd(new []{'a'}, LONG_STRING.Length - 1).ToArray());
            levenshteinCorrelater.AssertComparision(LONG_STRING, "a", expectedResult);
        }

        [TestMethod]
        public void Correlate_Strings2MuchLongerThanString1()
        {
            innerCorrelater.SetToRetrunInputCollection(0);

            var expectedResult = new CorrelaterResult<char>(-1, AddNullToEnd(new[] { 'a' }, LONG_STRING.Length - 1).ToArray(), LONG_STRING.ToCharArray());
            levenshteinCorrelater.AssertComparision("a", LONG_STRING, expectedResult);
        }

        [TestMethod]
        public void CorrelateIdenticalStrings()
        {
            var expectedResult = new CorrelaterResult<char>(0, LONG_STRING.ToCharArray(), LONG_STRING.ToCharArray());
            levenshteinCorrelater.AssertComparision(LONG_STRING, LONG_STRING, expectedResult);
        }

        [TestMethod]
        public void Correlate_FirstStringMissingOneChar()
        {
            var index = 2;
            var missingString = RemoveAtIndex(LONG_STRING, index);
            var expectedFirstArray = LONG_STRING.ToCharArray();
            expectedFirstArray[index] = '\0';
            var expectedResult = new CorrelaterResult<char>(-1, expectedFirstArray, LONG_STRING.ToCharArray());
            levenshteinCorrelater.AssertComparision(missingString, LONG_STRING, expectedResult);
        }

        [TestMethod]
        public void Correlate_SecondStringMissingOneChar()
        {
            var index = 2;
            var missingString = RemoveAtIndex(LONG_STRING, index);
            var expectedSecondArray = LONG_STRING.ToCharArray();
            expectedSecondArray[index] = '\0';
            var expectedResult = new CorrelaterResult<char>(-1, LONG_STRING.ToCharArray(), expectedSecondArray);
            levenshteinCorrelater.AssertComparision(LONG_STRING, missingString, expectedResult);
        }

        [TestMethod]
        public void OnProgressUpdatesHappensRightNumberOfTimes()
        {
            var correlater = new SplitToChunksCorrelaterWrapper<char>(innerCorrelater, CHUNK_SIZE);
            correlater.AssertProgressUpdateWasCalledRightNumberOfTimes(LONG_STRING.ToCharArray(), LONG_STRING.ToCharArray(), LONG_STRING.Length / CHUNK_SIZE + 1);
        }

        private string RemoveAtIndex(string s, int index)
        {
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
                if (i != index)
                    stringBuilder.Append(s[i]);

            return stringBuilder.ToString();
        }
        
        private IEnumerable<T> AddOffset<T>(IEnumerable<T> collection)
        {
            var result = new List<T> {default(T)};
            result.AddRange(collection);
            return result;
        }

        private IEnumerable<T> AddNullToEnd<T>(IEnumerable<T> collection) =>
            AddNullToEnd(collection, 1);

        private IEnumerable<T> AddNullToEnd<T>(IEnumerable<T> collection, int nulls)
        {
            var result = new List<T>();
            result.AddRange(collection);
            for (int i = 0; i < nulls; i++)
                result.Add(default(T));
            return result;
        }
    }
}
