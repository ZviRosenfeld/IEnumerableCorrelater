using System.Linq;
using IEnumerableCorrelater.Correlaters;
using IEnumerableCorrelater.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IEnumerableCorrelater.Benchmarking
{
    [TestClass]
    [TestCategory("Performance Benchmarking")]
    public class LevenshteinCorrelaterBenchmarking
    {
        private const int LENGTH = 7_500;
        private readonly ICorrelater<char> correlater = new LevenshteinCorrelater<char>(10, 7, 7);

        [TestMethod]
        public void LevenshteinCorrelater_BenchmarkTwoVeryLongStrings()
        {
            var string1 = Utils.GetLongString(LENGTH);
            var string2 = Utils.GetLongString(LENGTH);

            correlater.Correlate(string1, string2);
        }

        [TestMethod]
        public void LevenshteinCorrelater_BenchmarkTwoVeryBigCollections()
        {
            var collection1 = Utils.GetBigCollection(LENGTH);
            var collection2 = Utils.GetBigCollection(LENGTH);

            correlater.Correlate(collection1, collection2);
        }

        [TestMethod]
        public void LevenshteinCorrelater_BenchmarkTwoVeryBigAlomostSameCollections()
        {
            var collection1 = Utils.GetBigCollection(LENGTH);
            var collection2 = collection1.ToList();
            collection2[500] = collection1.ElementAt(500) == 'a' ? 'b' : 'a';

            correlater.Correlate(collection1, collection2);
        }
    }
}
