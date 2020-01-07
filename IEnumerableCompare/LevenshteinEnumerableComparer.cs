using System.Collections.Generic;
using System.Linq;
using IEnumerableCompare.Exceptions;
using IEnumerableCompare.Interfaces;

namespace IEnumerableCompare
{
    /// <summary>
    /// This class used dynamic programming to calculate the Levenshtein distance between two collections.
    /// </summary>
    public class LevenshteinEnumerableComparer<T> : IEnumerableComparer<T>
    {
        private readonly IDistanceCalculator<T> distanceCalculator;
        private readonly int removalCost;
        private readonly int insertionCost;

        public LevenshteinEnumerableComparer(IDistanceCalculator<T> distanceCalculator, int removalCost, int insertionCost)
        {
            if (default(T) != null)
                throw new EnumerableCompareException($"{nameof(T)} must be nullable");

            this.distanceCalculator = distanceCalculator;
            this.removalCost = removalCost;
            this.insertionCost = insertionCost;
        }

        public CompareResult<T> Compare(IEnumerable<T> enumerable1, IEnumerable<T> enumerable2) =>
            Compare(enumerable1.ToArray(), enumerable2.ToArray());

        public CompareResult<T> Compare(T[] array1, T[] array2)
        {
            var dynamicTable = CreateDynamicTable(array1, array2);
            var matchingArrays = GetMatchingArrays(dynamicTable, array1, array2);
            return new CompareResult<T>(dynamicTable[array1.Length, array2.Length], matchingArrays.Item1, matchingArrays.Item2);
        }

        /// <summary>
        /// Creates the dynamic table. 
        /// Cell[i, j] defines the best match in which array1 contains all elements up to i (including), and array2 contains all elements up to j (including)
        /// </summary>
        private int[,] CreateDynamicTable(T[] array1, T[] array2)
        {
            var dynamicTable = new int[array1.Length + 1, array2.Length + 1];

            for (int i = 0; i < array1.Length + 1; i++)
                dynamicTable[i, 0] = i * removalCost;
            for (int i = 0; i < array2.Length + 1; i++)
                dynamicTable[0, i] = i * insertionCost;

            for (int i = 1; i < array1.Length + 1; i++)
            for (int j = 1; j < array2.Length + 1; j++)
            {
                var substitution = dynamicTable[i - 1, j - 1] + distanceCalculator.Distance(array1[i - 1], array2[j - 1]);
                var insertion = dynamicTable[i, j - 1] + insertionCost;
                var removal = dynamicTable[i - 1, j] + removalCost;
                dynamicTable[i, j] = Min(substitution, insertion, removal);
            }
            return dynamicTable;
        }

        private (T[], T[]) GetMatchingArrays(int[,] dynamicTable, T[] array1, T[] array2)
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
                    bestMatchList2.Add(array2[j - 1]);
                    j--;
                }
                // In case we removed the element
                else if (dynamicTable[i, j] == dynamicTable[i - 1, j] + removalCost)
                {
                    bestMatchList1.Add(array1[i - 1]);
                    bestMatchList2.Add(default(T));
                    i--;
                }
                // In case we substituted the element
                else
                {
                    bestMatchList1.Add(array1[i - 1]);
                    bestMatchList2.Add(array2[j - 1]);
                    i--;
                    j--;
                }
            }
            if (i == 0 && j != 0)
            {
                for (; j > 0; j--)
                {
                    bestMatchList1.Add(default(T));
                    bestMatchList2.Add(array2[j - 1]);
                }
            }
            if (j == 0 && i != 0)
            {
                for (; i > 0; i--)
                {
                    bestMatchList1.Add(array1[i - 1]);
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
