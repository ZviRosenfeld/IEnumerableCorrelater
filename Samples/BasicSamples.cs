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

            // You'll need to implement your own IDistanceCalculator<T>. 
            // IDistanceCalculator defines the "distance" between any to elements.
            IDistanceCalculator<string> distanceCalculator = new MyDistanceCalculator<string>();

            // The library contains a number of ICorrelaters. 
            // LevenshteinCorrelater uses dynamic programing to find the Levenshtein-distance between the two collections.
            ICorrelater<string> correlater = 
                new LevenshteinCorrelater<string>(distanceCalculator, removalCost, insertionCost);

            // Wrap the ICorrelater with an EnumerableCorrelater<T> to use it to compare collections
            EnumerableCorrelater<string> enumerableCorrelater = new EnumerableCorrelater<string>(correlater);

            string[] array1 = { "A", "D", "C" };
            string[] array2 = { "A", "B", "C" };

            // Compare the collections - you can compare any IEnumerable<T>.
            CorrelaterResult<string> result = enumerableCorrelater.Correlate(array1, array2);

            // Print some of the result
            Console.WriteLine(result.Distance);
            Console.WriteLine(result.BestMatch1);
            Console.WriteLine(result.BestMatch2);

            return result;
        }

        public CorrelaterResult<char> StringCorrelaterExsample()
        {
            int removalCost = 1, insertionCost = 1;

            // You'll need to implement your own IDistanceCalculator<char>. 
            // IDistanceCalculator defines the "distance" between any to elements.
            IDistanceCalculator<char> distanceCalculator = new MyDistanceCalculator<char>();

            // The library contains a number of ICorrelaters. 
            // LevenshteinCorrelater uses dynamic programing to find the Levenshtein-distance between the two collections.
            ICorrelater<char> correlater = 
                new LevenshteinCorrelater<char>(distanceCalculator, removalCost, insertionCost);

            // Wrap the ICorrelater with a StringCorrelater to use it to compare strings
            StringCorrelater stringCorrelater = new StringCorrelater(correlater);

            string string1 = "ABC";
            string string2 = "ADC";

            // Compare the strings.
            CorrelaterResult<char> result = stringCorrelater.Correlate(string1, string2);

            // Print some of the result
            Console.WriteLine(result.Distance);
            Console.WriteLine(result.BestMatch1);
            Console.WriteLine(result.BestMatch2);

            return result;
        }
    }
}
