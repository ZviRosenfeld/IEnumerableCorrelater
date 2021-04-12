using IEnumerableCorrelater.Correlaters;
using IEnumerableCorrelater.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace IEnumerableCorrelater.Benchmarking
{
    [TestClass]
    [TestCategory("Performance Benchmarking")]
    public class PatienceDiffCorrelaterBenchmarking
    {
        private const int LENGTH = 20000;
        private readonly ICorrelater<char> correlater = new PatienceDiffCorrelater<char>();

        [TestMethod]
        public void PatienceDiffCorrelaterCorrelater_BenchmarkTwoVeryLongStrings()
        {
            var string1 = Utils.GetLongString(LENGTH);
            var string2 = Utils.GetLongString(LENGTH);

            var asciiCharCode = 1;
            for (var i = 1000; i < string1.Length; i += 1002, asciiCharCode++)
            {
                string1 = string1.Insert(i, ((char)asciiCharCode).ToString());
                string2 = string2.Insert(i, ((char)asciiCharCode).ToString());
            }    

            correlater.Correlate(string1, string2);
        }
    }
}
