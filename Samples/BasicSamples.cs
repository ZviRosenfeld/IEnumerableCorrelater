using IEnumerableCorrelater;
using System;
using IEnumerableCorrelater.Correlaters;
using IEnumerableCorrelater.Interfaces;

namespace Samples
{
    class BasicSamples
    {
        public CorrelaterResult<string> IEnumerableCorrelaterExsample()
        {
            int removalCost = 1, insertionCost = 1;
            IDistanceCalculator<string> distanceCalculator = new MyDistanceCalculator<string>();
            ICorrelater<string> correlater = 
                new LevenshteinCorrelater<string>(distanceCalculator, removalCost, insertionCost);
            EnumerableCorrelater<string> enumerableCorrelater = new EnumerableCorrelater<string>(correlater);

            string[] array1 = { "A", "D", "C" };
            string[] array2 = { "A", "B", "C" };

            CorrelaterResult<string> result = enumerableCorrelater.Correlate(array1, array2);

            // Print some of the results
            Console.WriteLine(result.Distance);
            Console.WriteLine(result.BestMatch1);
            Console.WriteLine(result.BestMatch2);

            return result;
        }

        public CorrelaterResult<char> StringCorrelaterExsample()
        {
            int removalCost = 1, insertionCost = 1;
            IDistanceCalculator<char> distanceCalculator = new MyDistanceCalculator<char>();
            ICorrelater<char> correlater = 
                new LevenshteinCorrelater<char>(distanceCalculator, removalCost, insertionCost);
            StringCorrelater stringCorrelater = new StringCorrelater(correlater);

            string string1 = "ABC";
            string string2 = "ADC";

            CorrelaterResult<char> result = stringCorrelater.Correlate(string1, string2);

            // Print some of the results
            Console.WriteLine(result.Distance);
            Console.WriteLine(result.BestMatch1);
            Console.WriteLine(result.BestMatch2);

            return result;
        }
    }
}
