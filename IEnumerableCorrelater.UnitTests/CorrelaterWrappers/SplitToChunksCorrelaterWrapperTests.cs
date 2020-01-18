using System;
using System.Collections.Generic;
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
        [DataRow(-1)]
        [ExpectedException(typeof(ArgumentException), "chunkSize")]
        public void Correlate_ChunkSizeSmallerThanOne_ThrowException(int chunkSize) =>
            new SplitToChunksCorrelaterWrapper<char>(null, chunkSize);

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [ExpectedException(typeof(ArgumentException), "maxDistance")]
        public void Correlate_MaxDistanceSmallerThanOne_ThrowException(int maxDistnace) =>
            new SplitToChunksCorrelaterWrapper<char>(null, 10, maxDistnace);

        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(6)]
        public void Correlate_ReturnsRightDistance(int distancePerChunk)
        {
            innerCorrelater.SetToRetrunInputCollection(distancePerChunk);

            var result = correlater.Correlate(LONG_STRING, LONG_STRING);
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

            var result = correlater.Correlate(LONG_STRING, LONG_STRING);
            result.BestMatch1.RemoveNull().AssertAreSame(result.BestMatch2.RemoveNull(), "Collection is missing elements");
        }

        [TestMethod]
        public void Correlate_MatchWithOffsetForSecond_DontMissPartOfMatch()
        {
            innerCorrelater.SetToRetrun(AddNullToEnd, AddOffset);

            var result = correlater.Correlate(LONG_STRING, LONG_STRING);
            result.BestMatch1.RemoveNull().AssertAreSame(result.BestMatch2.RemoveNull(), "Collection is missing elements");
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

        [TestMethod]
        public void OnResultUpdate_NoOffset_DontMissPartOfMatch()
        {
            innerCorrelater.SetToRetrunInputCollection(0);
            var correlater = new SplitToChunksCorrelaterWrapper<char>(innerCorrelater, CHUNK_SIZE);
            correlater.AssertOnResultUpdateWorks(LONG_STRING, LONG_STRING);
        }

        [TestMethod]
        public void OnResultUpdate_MatchWithOffsetForFirst_DontMissPartOfMatch()
        {
            innerCorrelater.SetToRetrun(AddOffset, AddNullToEnd);
            var correlater = new SplitToChunksCorrelaterWrapper<char>(innerCorrelater, CHUNK_SIZE);
            correlater.AssertOnResultUpdateWorks(LONG_STRING, LONG_STRING);
        }

        [TestMethod]
        public void OnResultUpdate_TotalyDiffrentStrings_DontMissPartOfMatch()
        {
            innerCorrelater.SetToRetrunInputCollection(0);
            var correlater = new SplitToChunksCorrelaterWrapper<char>(innerCorrelater, CHUNK_SIZE);
            correlater.AssertOnResultUpdateWorks("abcdefghijklmnopqrstuvwxyz", "1234567890");
        }

        [TestMethod]
        public void OnResultUpdate_OneCharMissing_DontMissPartOfMatch()
        {
            var correlater = new SplitToChunksCorrelaterWrapper<char>(new LevenshteinCorrelater<char>(10, 7, 7), CHUNK_SIZE);
            correlater.AssertOnResultUpdateWorks(LONG_STRING, LONG_STRING.Remove(8, 1));
        }

        [TestMethod]
        public void OnResultUpdate_ManyCharMissing_DontMissPartOfMatch()
        {
            var correlater = new SplitToChunksCorrelaterWrapper<char>(new LevenshteinCorrelater<char>(10, 7, 7), CHUNK_SIZE);
            correlater.AssertOnResultUpdateWorks(LONG_STRING, LONG_STRING.Remove(8, 9));
        }
        
        [TestMethod]
        [DataRow(1)]
        [DataRow(10)]
        public void Correlate_StopAtMaxDistance(int maxDistnace)
        {
            innerCorrelater.SetToRetrunInputCollection(5);
            var correlater = new SplitToChunksCorrelaterWrapper<char>(innerCorrelater, 6, maxDistnace);

            var result = correlater.Correlate(LONG_STRING, LONG_STRING);
            Assert.IsNull(result);
        }

        [TestMethod]
        [DataRow(30)]
        [DataRow(1000)]
        public void Correlate_StopAtMaxDistance_MaxDistnaceNotReached(int maxDistnace)
        {
            innerCorrelater.SetToRetrunInputCollection(5);
            var correlater = new SplitToChunksCorrelaterWrapper<char>(innerCorrelater, 6, maxDistnace);

            var result = correlater.Correlate(LONG_STRING, LONG_STRING);
            Assert.IsNotNull(result);
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
