using System.Collections.Generic;
using System.Linq;
using IEnumerableCorrelater.Exceptions;
using IEnumerableCorrelater.Interfaces;

namespace IEnumerableCorrelater.LevenshteinCorrelater
{
    /// <summary>
    /// A base class used to calculate the Levenshtein distance between two collections or strings.
    /// </summary>
    public class BaseLevenshteinCorrelater<T>
    {
        private readonly IDistanceCalculator<T> distanceCalculator;
        private readonly int removalCost;
        private readonly int insertionCost;

        /// <summary>
        /// This class used dynamic programming to calculate the Levenshtein distance between two collections.
        /// </summary>
        public BaseLevenshteinCorrelater(IDistanceCalculator<T> distanceCalculator, int removalCost, int insertionCost)
        {
            if (default(T) != null  && typeof(T) != typeof(char))
                throw new EnumerableCorrelaterException($"{nameof(T)} must be nullable");

            this.distanceCalculator = distanceCalculator;
            this.removalCost = removalCost;
            this.insertionCost = insertionCost;
        }

        public CorrelaterResult<T> Compare(ICollectionWrapper<T> collection1, ICollectionWrapper<T> collection2)
        {
            var dynamicTable = CreateDynamicTable(collection1, collection2);
            var matchingArrays = GetMatchingArrays(dynamicTable, collection1, collection2);
            return new CorrelaterResult<T>(dynamicTable[collection1.Length, collection2.Length], matchingArrays.Item1, matchingArrays.Item2);
        }

        /// <summary>
        /// Creates the dynamic table. 
        /// Cell[i, j] defines the best match in which array1 contains all elements up to i (including), and array2 contains all elements up to j (including)
        /// </summary>
        private int[,] CreateDynamicTable(ICollectionWrapper<T> collection1, ICollectionWrapper<T> collection2)
        {
            var dynamicTable = new int[collection1.Length + 1, collection2.Length + 1];

            for (int i = 0; i < collection1.Length + 1; i++)
                dynamicTable[i, 0] = i * removalCost;
            for (int i = 0; i < collection2.Length + 1; i++)
                dynamicTable[0, i] = i * insertionCost;

            for (int i = 1; i < collection1.Length + 1; i++)
                for (int j = 1; j < collection2.Length + 1; j++)
                {
                    var substitution = dynamicTable[i - 1, j - 1] + distanceCalculator.Distance(collection1[i - 1], collection2[j - 1]);
                    var insertion = dynamicTable[i, j - 1] + insertionCost;
                    var removal = dynamicTable[i - 1, j] + removalCost;
                    dynamicTable[i, j] = Min(substitution, insertion, removal);
                }
            return dynamicTable;
        }

        private (T[], T[]) GetMatchingArrays(int[,] dynamicTable, ICollectionWrapper<T> collection1, ICollectionWrapper<T> collection2)
        {
            var bestMatchList1 = new List<T>();
            var bestMatchList2 = new List<T>();

            int i = dynamicTable.GetLength(0) - 1, j = dynamicTable.GetLength(1) - 1;
            while (i > 0 && j > 0)
            {
                // In case we inserted the element
                if (dynamicTable[i, j] == dynamicTable[i, j - 1] + insertionCost)
                {
                    bestMatchList1.Add(default(T));
                    bestMatchList2.Add(collection2[j - 1]);
                    j--;
                }
                // In case we removed the element
                else if (dynamicTable[i, j] == dynamicTable[i - 1, j] + removalCost)
                {
                    bestMatchList1.Add(collection1[i - 1]);
                    bestMatchList2.Add(default(T));
                    i--;
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

        private int Min(params int[] args) => args.Min();
    }
}
