using IEnumerableCorrelater.Correlaters;
using IEnumerableCorrelater.CorrelaterWrappers;
using IEnumerableCorrelater.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IEnumerableCorrelater.Benchmarking
{
    [TestClass]
    [TestCategory("Performance Benchmarking")]
    public class SplitToChunksCorrelaterWrapperBenchmarking
    {
        private const int LENGTH = 100000;
        private const int CHUNK_SIZE = 200;
        private readonly ICorrelater<char> correlater = new SplitToChunksCorrelaterWrapper<char>(new LevenshteinCorrelater<char>(10, 7, 7), CHUNK_SIZE);

        [TestMethod]
        public void SplitToChunksCorrelater_BenchmarkTwoVeryLongStrings()
        {
            var string1 = Utils.GetLongString(LENGTH);
            var string2 = Utils.GetLongString(LENGTH);

            var stringCorrelater = new StringCorrelater(correlater);
            stringCorrelater.Correlate(string1, string2);
        }
    }
}
