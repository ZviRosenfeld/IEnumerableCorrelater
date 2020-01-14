﻿using System;
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
        
        public LevenshteinCorrelater(IDistanceCalculator<T> distanceCalculator, int removalCost, int insertionCost)
        {
            correlater = new DamerauLevenshteinCorrelater<T>(distanceCalculator, null, removalCost, insertionCost);
            correlater.OnProgressUpdate += (p, t) => OnProgressUpdate?.Invoke(p, t);
        }

        public LevenshteinCorrelater(int substitutionCost, int removalCost, int insertionCost)
        {
            correlater = new DamerauLevenshteinCorrelater<T>(new BasicDistanceCalculator<T>(substitutionCost), null, removalCost, insertionCost);
            correlater.OnProgressUpdate += (p, t) => OnProgressUpdate?.Invoke(p, t);
        }

        public LevenshteinCorrelater(IDistanceCalculator<T> distanceCalculator, IRemovalCalculator<T> removalCalculator, IInsertionCalculator<T> insertionCalculator)
        {
            correlater = new DamerauLevenshteinCorrelater<T>(distanceCalculator, null, removalCalculator, insertionCalculator);
            correlater.OnProgressUpdate += (p, t) => OnProgressUpdate?.Invoke(p, t);
        }

        public CorrelaterResult<T> Compare(ICollectionWrapper<T> collection1, ICollectionWrapper<T> collection2) =>
            correlater.Compare(collection1, collection2);

        public event Action<int, int> OnProgressUpdate;
    }
}
