﻿using IEnumerableCorrelater;
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
            int chunkSize = 200; // Bigger chunks will result in a slower, but more acurate correlation
            ICorrelater<char> innerCorrelater = 
                new LevenshteinCorrelater<char>(missmatchCost, removalCost, insertionCost);

            // The SplitToChunksCorrelaterWrapper wrappes an inner ICorrelater
            ICorrelater<char> splitToChunksCorrelaterWrapper =
                new SplitToChunksCorrelaterWrapper<char>(innerCorrelater, chunkSize);

            // Wrap the ICorrelater with an EnumerableCorrelater<T> to use it to compare collections
            EnumerableCorrelater<char> enumerableCorrelater =
                new EnumerableCorrelater<char>(splitToChunksCorrelaterWrapper);

            CorrelaterResult<char> result = enumerableCorrelater.Correlate(collection1, collection2);

            return result;
        }
    }
}