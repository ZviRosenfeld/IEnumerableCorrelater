using IEnumerableCorrelater.Calculators;
using IEnumerableCorrelater.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;

namespace IEnumerableCorrelater.Correlaters
{
    /// <summary>
    /// Uses dynamic programming to solve the longest common subsequence (LCS) problem. 
    /// </summary>
    public class DynamicLcsCorrelater<T> : ICorrelater<T>
    {
        private readonly DamerauLevenshteinCorrelater<T> correlater;

        public DynamicLcsCorrelater() : this(1 ,1)
        {
        }

        public DynamicLcsCorrelater(uint removalCost, uint insertionCost) : 
            this(new BasicRemovalCalculator<T>(removalCost), new BasicInsertionCalculator<T>(insertionCost))
        {
        }

        public DynamicLcsCorrelater(IRemovalCalculator<T> removalCalculator, IInsertionCalculator<T> insertionCalculator)
        {
            correlater = new DamerauLevenshteinCorrelater<T>(null, null, removalCalculator, insertionCalculator);
            correlater.OnProgressUpdate += (p, t) => OnProgressUpdate?.Invoke(p, t);
        }

        public CorrelaterResult<T> Correlate(IEnumerable<T> collection1, IEnumerable<T> collection2, CancellationToken cancellationToken = default) =>
            correlater.Correlate(collection1, collection2, cancellationToken);

        public event Action<int, int> OnProgressUpdate;
    }
}
