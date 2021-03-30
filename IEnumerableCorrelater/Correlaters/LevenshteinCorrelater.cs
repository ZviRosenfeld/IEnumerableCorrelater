using System;
using System.Collections.Generic;
using IEnumerableCorrelater.Calculators;
using IEnumerableCorrelater.Interfaces;

namespace IEnumerableCorrelater.Correlaters
{
    /// <summary>
    /// A base class used to calculate the Levenshtein distance between two collections or strings.
    /// </summary>
    public class LevenshteinCorrelater<T> : ICorrelater<T>
    {
        private readonly DamerauLevenshteinCorrelater<T> correlater;
        
        public LevenshteinCorrelater(IDistanceCalculator<T> distanceCalculator, uint removalCost, uint insertionCost)
        {
            correlater = new DamerauLevenshteinCorrelater<T>(distanceCalculator, null, removalCost, insertionCost);
            correlater.OnProgressUpdate += (p, t) => OnProgressUpdate?.Invoke(p, t);
        }

        public LevenshteinCorrelater(uint substitutionCost, uint removalCost, uint insertionCost)
        {
            correlater = new DamerauLevenshteinCorrelater<T>(new BasicDistanceCalculator<T>(substitutionCost), null, removalCost, insertionCost);
            correlater.OnProgressUpdate += (p, t) => OnProgressUpdate?.Invoke(p, t);
        }

        public LevenshteinCorrelater(IDistanceCalculator<T> distanceCalculator, IRemovalCalculator<T> removalCalculator, IInsertionCalculator<T> insertionCalculator)
        {
            correlater = new DamerauLevenshteinCorrelater<T>(distanceCalculator, null, removalCalculator, insertionCalculator);
            correlater.OnProgressUpdate += (p, t) => OnProgressUpdate?.Invoke(p, t);
        }

        public CorrelaterResult<T> Correlate(IEnumerable<T> collection1, IEnumerable<T> collection2) =>
            correlater.Correlate(collection1, collection2);

        public event Action<int, int> OnProgressUpdate;
    }
}
