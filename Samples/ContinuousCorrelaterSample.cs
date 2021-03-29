using System.Threading.Tasks;
using IEnumerableCorrelater;
using IEnumerableCorrelater.Correlaters;
using IEnumerableCorrelater.CorrelaterWrappers;
using IEnumerableCorrelater.Interfaces;

namespace Samples
{
    class ContinuousCorrelaterSample
    {
        char[] collection1 = "ABC".ToCharArray();
        char[] collection2 = "ABB".ToCharArray();
        ICorrelater<char> innerCorrelater = new LevenshteinCorrelater<char>(10, 7, 7);
        private int chunkSize = 200;
        MyUi<char> myUi = new MyUi<char>();

        public void Sample1()
        {
            IContinuousCorrelater<char> continuousCorrelater =
                new SplitToChunksCorrelaterWrapper<char>(innerCorrelater, chunkSize);
            continuousCorrelater.OnResultUpdate += (CorrelaterResult<char> partialResult) =>
            {
                // Do something with the results here.
                // Please note that the OnResultUpdate will only contain the new segment (and not previously sent segments).

                myUi.Distance += partialResult.Distance; // Note that the accumulated distance may differ from the actual distance.
                myUi.BestMatch1.AddRange(partialResult.BestMatch1);
                myUi.BestMatch2.AddRange(partialResult.BestMatch2);
            };
            
            // Run the correlate in a new thread so that our UI doesn't freeze
            Task.Run(() => continuousCorrelater.Correlate(collection1, collection2));
        }
    }
}
