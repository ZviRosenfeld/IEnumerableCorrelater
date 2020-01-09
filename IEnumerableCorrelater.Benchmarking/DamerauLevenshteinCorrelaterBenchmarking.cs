using System.Linq;
using IEnumerableCorrelater.Correlaters;
using IEnumerableCorrelater.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IEnumerableCorrelater.Benchmarking
{
    [TestClass]
    [TestCategory("Benchmarking")]
    public class DamerauLevenshteinCorrelaterBenchmarking
    {
        private const int LENGTH = 7500;
        private readonly ICorrelater<char> correlater = new DamerauLevenshteinCorrelater<char>(10, 15, 8, 8);

        [TestMethod]
        public void DamerauLevenshteinCorrelater_BenchmarkTwoVeryLongStrings()
        {
            var string1 = Utils.GetLongString(LENGTH);
            var string2 = Utils.GetLongString(LENGTH);

            var stringCorrelater = new StringCorrelater(correlater);
            stringCorrelater.Correlate(string1, string2);
        }

        [TestMethod]
        public void DamerauLevenshteinCorrelater_BenchmarkTwoVeryBigCollections()
        {
            var collection1 = Utils.GetBigCollection(LENGTH);
            var collection2 = Utils.GetBigCollection(LENGTH);

            var enumerableCorrelater = new EnumerableCorrelater<char>(correlater);
            enumerableCorrelater.Correlate(collection1, collection2);
        }

        [TestMethod]
        public void DamerauLevenshteinCorrelater_BenchmarkTwoVeryBigAlomostSameCollections()
        {
            var collection1 = Utils.GetBigCollection(LENGTH);
            var collection2 = collection1.ToList();
            collection2[500] = collection1.ElementAt(500) == 'a' ? 'b' : 'a';

            var enumerableCorrelater = new EnumerableCorrelater<char>(correlater);
            enumerableCorrelater.Correlate(collection1, collection2);
        }
    }
}
