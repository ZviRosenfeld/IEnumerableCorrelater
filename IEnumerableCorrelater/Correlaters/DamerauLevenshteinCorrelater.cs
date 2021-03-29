using System;
using System.Collections.Generic;
using System.Linq;
using IEnumerableCorrelater.Calculators;
using IEnumerableCorrelater.CollectionWrappers;
using IEnumerableCorrelater.Exceptions;
using IEnumerableCorrelater.Interfaces;

namespace IEnumerableCorrelater.Correlaters
{
    public class DamerauLevenshteinCorrelater<T> : ICorrelater<T>
    {
        private readonly IDistanceCalculator<T> distanceCalculator;
        private readonly ITranspositionCalculator<T> transpositionCalculator;
        private readonly IRemovalCalculator<T> removalCalculator;
        private readonly IInsertionCalculator<T> insertionCalculator;
        
        public DamerauLevenshteinCorrelater(IDistanceCalculator<T> distanceCalculator, ITranspositionCalculator<T> transpositionCalculator, int removalCost, int insertionCost) :
            this(distanceCalculator, transpositionCalculator, new BasicRemovalCalculator<T>(removalCost), new BasicInsertionCalculator<T>(insertionCost))
        {
        }
        
        public DamerauLevenshteinCorrelater(int substitutionCost, int transpositionCost, int removalCost, int insertionCost) :
            this (new BasicDistanceCalculator<T>(substitutionCost), new BasicTranspositionCalculator<T>(transpositionCost), removalCost, insertionCost)
        {
        }

        public DamerauLevenshteinCorrelater(IDistanceCalculator<T> distanceCalculator, ITranspositionCalculator<T> transpositionCalculator, IRemovalCalculator<T> removalCalculator, IInsertionCalculator<T> insertionCalculator)
        {
            if (default(T) != null && typeof(T) != typeof(char))
                throw new EnumerableCorrelaterException($"{nameof(T)} must be nullable or a char");

            this.distanceCalculator = distanceCalculator;
            this.transpositionCalculator = transpositionCalculator;
            this.removalCalculator = removalCalculator;
            this.insertionCalculator = insertionCalculator;
        }

        private CorrelaterResult<T> Compare(ICollectionWrapper<T> collection1, ICollectionWrapper<T> collection2)
        {
            var dynamicTable = CreateDynamicTable(collection1, collection2);
            var matchingArrays = GetMatchingArrays(dynamicTable, collection1, collection2);
            return new CorrelaterResult<T>(dynamicTable[collection1.Length, collection2.Length], matchingArrays.Item1, matchingArrays.Item2);
        }

        public CorrelaterResult<T> Correlate(IEnumerable<T> collection1, IEnumerable<T> collection2)
        {
            var collection1Wrapper = collection1.ToCollectionWrapper();
            var collection2Wrapper = collection2.ToCollectionWrapper();

            collection1Wrapper.CheckForNulls(nameof(collection1));
            collection2Wrapper.CheckForNulls(nameof(collection2));

            return Compare(collection1Wrapper, collection2Wrapper);
        }

        public event Action<int, int> OnProgressUpdate;

        /// <summary>
        /// Creates the dynamic table. 
        /// Cell[i, j] defines the best match in which array1 contains all elements up to i (including), and array2 contains all elements up to j (including)
        /// </summary>
        private long[,] CreateDynamicTable(ICollectionWrapper<T> collection1, ICollectionWrapper<T> collection2)
        {
            var dynamicTable = new long[collection1.Length + 1, collection2.Length + 1];

            dynamicTable[0, 0] = 0;
            for (int i = 1; i < collection1.Length + 1; i++)
                dynamicTable[i, 0] = dynamicTable[i - 1, 0] + removalCalculator.RemovalCost(collection1[i - 1]);
            for (int i = 1; i < collection2.Length + 1; i++)
                dynamicTable[0, i] = dynamicTable[0, i - 1] + insertionCalculator.InsertionCost(collection2[i - 1]);

            OnProgressUpdate?.Invoke(1, collection1.Length + 1);
            for (int i = 1; i < collection1.Length + 1; i++)
            {
                for (int j = 1; j < collection2.Length + 1; j++)
                {
                    if (collection1[i - 1].Equals(collection2[j - 1]))
                    {
                        dynamicTable[i, j] = dynamicTable[i - 1, j - 1];
                        continue;
                    }

                    var substitution = dynamicTable[i - 1, j - 1] + distanceCalculator.Distance(collection1[i - 1], collection2[j - 1]);
                    var insertion = dynamicTable[i, j - 1] + insertionCalculator.InsertionCost(collection1[i - 1]);
                    var removal = dynamicTable[i - 1, j] + removalCalculator.RemovalCost(collection2[j - 1]);
                    var min = Min(substitution, insertion, removal);
                    if (CanDoTransposition(collection1, collection2, i, j))
                    {
                        var transposition = transpositionCalculator.TranspositionCost(collection1[i - 1], collection1[i - 2]) + dynamicTable[i - 2, j - 2];
                        min = Math.Min(min, transposition);
                    }
                    dynamicTable[i, j] = min;
                }
                OnProgressUpdate?.Invoke(i + 1, collection1.Length + 1);
            }
            return dynamicTable;
        }

        private bool CanDoTransposition(ICollectionWrapper<T> collection1, ICollectionWrapper<T> collection2, int i, int j)
        {
            if (transpositionCalculator == null) return false;
            if (i < 2 || j < 2) return false;
            return (collection1[i - 1].Equals(collection2[j - 2]) && collection1[i - 2].Equals(collection2[j - 1]));
        }

        private (T[], T[]) GetMatchingArrays(long[,] dynamicTable, ICollectionWrapper<T> collection1, ICollectionWrapper<T> collection2)
        {
            var bestMatchList1 = new List<T>();
            var bestMatchList2 = new List<T>();

            int i = dynamicTable.GetLength(0) - 1, j = dynamicTable.GetLength(1) - 1;
            while (i > 0 && j > 0)
            {
                // In case we removed the element
                if (dynamicTable[i, j] == dynamicTable[i - 1, j] + removalCalculator.RemovalCost(collection1[i - 1]))
                {
                    bestMatchList1.Add(collection1[i - 1]);
                    bestMatchList2.Add(default(T));
                    i--;
                }
                // In case we inserted the element
                else if (dynamicTable[i, j] == dynamicTable[i, j - 1] + insertionCalculator.InsertionCost(collection2[j - 1]))
                {
                    bestMatchList1.Add(default(T));
                    bestMatchList2.Add(collection2[j - 1]);
                    j--;
                }
                // In case we transposited the element
                else if (CanDoTransposition(collection1, collection2, i, j) &&
                         dynamicTable[i, j] == transpositionCalculator.TranspositionCost(collection1[i - 1], collection1[i - 2]) + dynamicTable[i - 2, j - 2])
                {
                    bestMatchList1.Add(collection1[i - 1]);
                    bestMatchList1.Add(collection1[i - 2]);
                    bestMatchList2.Add(collection2[j - 1]);
                    bestMatchList2.Add(collection2[j - 2]);
                    i -= 2;
                    j -= 2;
                }
                // In case we substituted the element
                else
                {
                    bestMatchList1.Add(collection1[i - 1]);
                    bestMatchList2.Add(collection2[j - 1]);
                    i--;
                    j--;
                }
            }
            if (i == 0 && j != 0)
            {
                for (; j > 0; j--)
                {
                    bestMatchList1.Add(default(T));
                    bestMatchList2.Add(collection2[j - 1]);
                }
            }
            if (j == 0 && i != 0)
            {
                for (; i > 0; i--)
                {
                    bestMatchList1.Add(collection1[i - 1]);
                    bestMatchList2.Add(default(T));
                }
            }

            bestMatchList1.Reverse();
            bestMatchList2.Reverse();
            return (bestMatchList1.ToArray(), bestMatchList2.ToArray());
        }

        private long Min(params long[] args) => args.Min();
    }
}
