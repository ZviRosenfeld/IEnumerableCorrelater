using System.Collections.Generic;
using System.Linq;

namespace IEnumerableCorrelater.Utils
{
    /// <summary>
    /// Gets an array of unique numbers and returns the biggest rising sequence of elements in that array.
    /// </summary>
    public class PatienceSortingAlgorithm
    {
        public int[] GetRisingIndexes(int[] array)
        {
            if (!array.Any())
                return new int[0];

            var heaps = new List<int>();
            var pointers = new Dictionary<int, int>();

            heaps.Add(array[0]);
            for (var i = 1; i < array.Length; i++)
                AddElementToHeap(array[i], heaps, pointers);

            var result = new List<int>() { heaps.Last() };
            while (pointers.ContainsKey(result.Last()))
                result.Add(pointers[result.Last()]);

            result.Reverse();
            return result.ToArray();
        }

        private void AddElementToHeap(int element, List<int> heaps, Dictionary<int, int> pointers)
        {
            for (var j = 0; j < heaps.Count; j++)
            {
                if (element < heaps[j])
                {
                    heaps[j] = element;
                    if (j > 0)
                        pointers[element] = heaps[j - 1];
                    return;
                }
            }

            pointers[element] = heaps[heaps.Count - 1];
            heaps.Add(element);
        }
    }
}
