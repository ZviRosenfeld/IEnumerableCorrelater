using System.Collections.Generic;
using System.Linq;
using IEnumerableCompare.Interfaces;

namespace IEnumerableCompare
{
    public class LevenshteinEnumerableComparer<T> : IEnumerableComparer<T>
    {
        private readonly IDistanceCalculator<T> distanceCalculator;
        private readonly int removalCost;
        private readonly int insertionCost;

        public LevenshteinEnumerableComparer(IDistanceCalculator<T> distanceCalculator, int removalCost, int insertionCost)
        {
            this.distanceCalculator = distanceCalculator;
            this.removalCost = removalCost;
            this.insertionCost = insertionCost;
        }

        public CompareResult Compare(IEnumerable<T> enumerable1, IEnumerable<T> enumerable2) =>
            Compare(enumerable1.ToArray(), enumerable2.ToArray());

        public CompareResult Compare(T[] array1, T[] array2)
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

            return new CompareResult(dynamicTable[array1.Length, array2.Length]);
        }

        private int Min(params int[] args) => args.Min();
    }
}
