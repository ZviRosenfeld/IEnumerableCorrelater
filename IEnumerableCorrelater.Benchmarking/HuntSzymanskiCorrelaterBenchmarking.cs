using IEnumerableCorrelater.Correlaters;
using IEnumerableCorrelater.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace IEnumerableCorrelater.Benchmarking
{
    [TestClass]
    [TestCategory("Performance Benchmarking")]
    public class HuntSzymanskiCorrelaterBenchmarking
    {
        private const int LENGTH = 6000;
        private readonly ICorrelater<char> correlater = new HuntSzymanskiCorrelater<char>();

        [TestMethod]
        public void LevenshteinCorrelater_BenchmarkTwoVeryBigAlomostSameCollections()
        {
            var collection1 = Utils.GetBigCollection(LENGTH);
            var collection2 = collection1.ToList();
            collection2[500] = collection1.ElementAt(500) == 'a' ? 'b' : 'a';

            var t = correlater.Correlate(collection1, collection2);
        }
    }
}
