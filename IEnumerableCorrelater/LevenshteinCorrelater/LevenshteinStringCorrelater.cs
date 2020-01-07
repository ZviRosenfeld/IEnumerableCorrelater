using IEnumerableCorrelater.CollectionWrappers;
using IEnumerableCorrelater.Interfaces;

namespace IEnumerableCorrelater.LevenshteinCorrelater
{
    public class LevenshteinStringCorrelater : IStringCorrelater
    {
        private readonly BaseLevenshteinCorrelater<char> baseLevenshteinComparer;

        /// <summary>
        /// This class used dynamic programming to calculate the Levenshtein distance between two collections.
        /// </summary>
        public LevenshteinStringCorrelater(IDistanceCalculator<char> distanceCalculator, int removalCost, int insertionCost)
        {
            baseLevenshteinComparer = new BaseLevenshteinCorrelater<char>(distanceCalculator, removalCost, insertionCost);
        }

        public CorrelaterResult<char> Correlate(string string1, string string2) =>
            baseLevenshteinComparer.Compare(new StringCollectionWrapper(string1), new StringCollectionWrapper(string2));
    }
}
