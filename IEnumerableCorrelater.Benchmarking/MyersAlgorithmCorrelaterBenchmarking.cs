using IEnumerableCorrelater.Correlaters;
using IEnumerableCorrelater.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace IEnumerableCorrelater.Benchmarking
{
    [TestClass]
    [TestCategory("Performance Benchmarking")]
    public class MyersAlgorithmCorrelaterBenchmarking
    {
        private const int LENGTH = 7500;
        private readonly ICorrelater<char> correlater = new MyersAlgorithmCorrelater<char>();

        [TestMethod]
        public void MyersAlgorithmCorrelater_BenchmarkTwoVeryLongStrings()
        {
            var string1 = Utils.GetLongString(LENGTH);
            var string2 = Utils.GetLongString(LENGTH);

            correlater.Correlate(string1, string2);
        }

        [TestMethod]
        public void MyersAlgorithmCorrelater_BenchmarkTwoVeryBigAlomostSameCollections()
        {
            var collection1 = Utils.GetBigCollection(LENGTH);
            var collection2 = collection1.ToList();
            collection2[500] = collection1.ElementAt(500) == 'a' ? 'b' : 'a';

            correlater.Correlate(collection1, collection2);
        }
    }
}
