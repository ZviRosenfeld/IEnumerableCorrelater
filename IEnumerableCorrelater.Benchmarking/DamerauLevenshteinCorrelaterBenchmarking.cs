using IEnumerableCorrelater.Correlaters;
using IEnumerableCorrelater.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IEnumerableCorrelater.Benchmarking
{
    [TestClass]
    [TestCategory("Performance Benchmarking")]
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
    }
}
