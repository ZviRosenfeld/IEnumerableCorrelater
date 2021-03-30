using IEnumerableCorrelater.Correlaters;
using IEnumerableCorrelater.CorrelaterWrappers;
using IEnumerableCorrelater.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace IEnumerableCorrelater.Benchmarking
{
    [TestClass]
    [TestCategory("Performance Benchmarking")]
    public class IgnoreIdenticalBeginningAndEndCorrelaterWrapperBenchmarking
    {
        private const int LENGTH = 7_500;

        [TestMethod]
        public void LevenshteinCorrelater_BenchmarkTwoVeryBigAlomostSameCollections()
        {
            var collection1 = Utils.GetBigCollection(LENGTH);
            var collection2 = collection1.ToList();
            collection2[3_000] = collection1.ElementAt(3_000) == 'a' ? 'b' : 'a';

            ICorrelater<char> correlater = new LevenshteinCorrelater<char>(10, 7, 7);
            correlater = new IgnoreIdenticalBeginningAndEndCorrelaterWrapper<char>(correlater);
            correlater.Correlate(collection1, collection2);
        }
    }
}
