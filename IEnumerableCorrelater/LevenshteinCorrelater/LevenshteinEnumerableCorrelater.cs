using System.Collections.Generic;
using System.Linq;
using IEnumerableCorrelater.CollectionWrappers;
using IEnumerableCorrelater.Interfaces;

namespace IEnumerableCorrelater.LevenshteinCorrelater
{
    /// <summary>
    /// This class used dynamic programming to calculate the Levenshtein distance between two collections.
    /// </summary>
    public class LevenshteinEnumerableCorrelater<T> : IEnumerableCorrelater<T>
    {
        private readonly BaseLevenshteinCorrelater<T> baseLevenshteinComparer;

        /// <summary>
        /// This class used dynamic programming to calculate the Levenshtein distance between two collections.
        /// </summary>
        public LevenshteinEnumerableCorrelater(IDistanceCalculator<T> distanceCalculator, int removalCost, int insertionCost)
        {
            baseLevenshteinComparer = new BaseLevenshteinCorrelater<T>(distanceCalculator, removalCost, insertionCost);
        }

        public CorrelaterResult<T> Correlate(IEnumerable<T> enumerable1, IEnumerable<T> enumerable2) =>
            baseLevenshteinComparer.Compare(new ArrayCollectionWrapper<T>(enumerable1.ToArray()), new ArrayCollectionWrapper<T>(enumerable2.ToArray()));

        public CorrelaterResult<T> Correlate(T[] array1, T[] array2) =>
            baseLevenshteinComparer.Compare(new ArrayCollectionWrapper<T>(array1), new ArrayCollectionWrapper<T>(array2));
    }
}
