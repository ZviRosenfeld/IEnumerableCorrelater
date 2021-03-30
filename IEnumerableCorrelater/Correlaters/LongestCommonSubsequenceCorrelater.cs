using IEnumerableCorrelater.Interfaces;
using System;
using System.Collections.Generic;

namespace IEnumerableCorrelater.Correlaters
{
    /// <summary>
    /// Uses dynamic programming to solve the longest common subsequence (LCS) problem. 
    /// </summary>
    public class LongestCommonSubsequenceCorrelater<T> : ICorrelater<T>
    {
        private readonly DamerauLevenshteinCorrelater<T> correlater;

        public LongestCommonSubsequenceCorrelater(uint removalCost, uint insertionCost)
        {
            correlater = new DamerauLevenshteinCorrelater<T>(null, null, removalCost, insertionCost);
            correlater.OnProgressUpdate += (p, t) => OnProgressUpdate?.Invoke(p, t);
        }

        public LongestCommonSubsequenceCorrelater(IRemovalCalculator<T> removalCalculator, IInsertionCalculator<T> insertionCalculator)
        {
            correlater = new DamerauLevenshteinCorrelater<T>(null, null, removalCalculator, insertionCalculator);
            correlater.OnProgressUpdate += (p, t) => OnProgressUpdate?.Invoke(p, t);
        }

        public CorrelaterResult<T> Correlate(IEnumerable<T> collection1, IEnumerable<T> collection2) =>
            correlater.Correlate(collection1, collection2);

        public event Action<int, int> OnProgressUpdate;
    }
}
