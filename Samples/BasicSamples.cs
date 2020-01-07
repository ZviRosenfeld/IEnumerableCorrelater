using System;
using IEnumerableCompare;
using IEnumerableCompare.Interfaces;

namespace Samples
{
    class BasicSamples
    {
        public CompareResult<string> Exsample1()
        {
            int removalCost = 1, insertionCost = 1;
            IDistanceCalculator<string> distanceCalculator = new MyDistanceCalculator();
            IEnumerableComparer<string> comparer =
                new LevenshteinEnumerableComparer<string>(distanceCalculator, removalCost, insertionCost);

            string[] array1 = { "A", "D", "C" };
            string[] array2 = { "A", "B", "C" };

            CompareResult<string> result = comparer.Compare(array1, array2);

            // Print some of the results
            Console.WriteLine(result.Distance);
            Console.WriteLine(result.BestMatch1);
            Console.WriteLine(result.BestMatch1);

            return result;
        }
    }
}
