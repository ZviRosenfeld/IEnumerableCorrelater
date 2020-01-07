using IEnumerableCorrelater;
using System;
using IEnumerableCorrelater.Interfaces;
using IEnumerableCorrelater.LevenshteinCorrelater;

namespace Samples
{
    class BasicSamples
    {
        public CorrelaterResult<string> IEnumerableComparerExsample()
        {
            int removalCost = 1, insertionCost = 1;
            IDistanceCalculator<string> distanceCalculator = new MyDistanceCalculator<string>();
            IEnumerableCorrelater<string> comparer =
                new LevenshteinEnumerableCorrelater<string>(distanceCalculator, removalCost, insertionCost);

            string[] array1 = { "A", "D", "C" };
            string[] array2 = { "A", "B", "C" };

            CorrelaterResult<string> result = comparer.Compare(array1, array2);

            // Print some of the results
            Console.WriteLine(result.Distance);
            Console.WriteLine(result.BestMatch1);
            Console.WriteLine(result.BestMatch2);

            return result;
        }

        public CorrelaterResult<char> StringComparerExsample()
        {
            int removalCost = 1, insertionCost = 1;
            IDistanceCalculator<char> distanceCalculator = new MyDistanceCalculator<char>();
            IStringCorrelater comparer =
                new LevenshteinStringCorrelater(distanceCalculator, removalCost, insertionCost);

            string string1 = "ABC";
            string string2 = "ADC";

            CorrelaterResult<char> result = comparer.Compare(string1, string2);

            // Print some of the results
            Console.WriteLine(result.Distance);
            Console.WriteLine(result.BestMatch1);
            Console.WriteLine(result.BestMatch2);

            return result;
        }
    }
}
