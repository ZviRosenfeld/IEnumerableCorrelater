using IEnumerableCorrelater;
using IEnumerableCorrelater.Correlaters;
using IEnumerableCorrelater.CorrelaterWrappers;
using IEnumerableCorrelater.Interfaces;

namespace Samples
{
    class SplitToChunksCorrelaterWrapperSamples
    {
        char[] collection1 = "ABC".ToCharArray();
        char[] collection2 = "ABB".ToCharArray();

        public CorrelaterResult<char> SplitToChunksCorrelaterWrapper()
        {
            int removalCost = 7;
            int insertionCost = 7;
            int missmatchCost = 10;
            int chunkSize = 200; // Bigger chunks will result in a slower, albeit more accurate, correlation
            ICorrelater<char> innerCorrelater = 
                new LevenshteinCorrelater<char>(missmatchCost, removalCost, insertionCost);

            // The SplitToChunksCorrelaterWrapper wraps an inner ICorrelater
            ICorrelater<char> splitToChunksCorrelaterWrapper =
                new SplitToChunksCorrelaterWrapper<char>(innerCorrelater, chunkSize);
            
            CorrelaterResult<char> result = splitToChunksCorrelaterWrapper.Correlate(collection1, collection2);

            return result;
        }
    }
}
