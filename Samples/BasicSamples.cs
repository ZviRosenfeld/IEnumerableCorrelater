﻿using IEnumerableCorrelater;
using System;
using IEnumerableCorrelater.Correlaters;
using IEnumerableCorrelater.Interfaces;

namespace Samples
{
    class BasicSamples
    {
        public CorrelaterResult<string> IEnumerableCorrelaterExsample()
        {
            // The library contains a number of ICorrelaters. 
            // MyersAlgorithmCorrelater is particularity good for cases where we aren't expecting many changes (like diff tools for code changes). 
            // Indeed, it is used as the default diff algorithm for git.
            ICorrelater<string> correlater = new MyersAlgorithmCorrelater<string>();
            
            string[] array1 = { "A", "D", "C" };
            string[] array2 = { "A", "B", "C" };

            // Correlate the collections - you can compare any IEnumerable<T>.
            CorrelaterResult<string> result = correlater.Correlate(array1, array2);

            // Print some of the result
            Console.WriteLine(result.Distance); // Should be 2
            Console.WriteLine(result.BestMatch1); // Should be { "A", "D", null, "C"}
            Console.WriteLine(result.BestMatch2); // Should be { "A", null, "B", "C"}

            return result;
        }

        public CorrelaterResult<char> StringCorrelaterExsample()
        {
            uint removalCost = 1, insertionCost = 1;

            // You'll need to implement your own IDistanceCalculator<char>. 
            // IDistanceCalculator defines the "distance" between any two elements.
            IDistanceCalculator<char> distanceCalculator = new MyDistanceCalculator<char>();

            // The library contains a number of ICorrelaters. 
            // LevenshteinCorrelater uses dynamic programing to find the Levenshtein-distance between the two collections.
            // Since a string is actually an IEnumerable<char>, we need to use an ICorrelater<char>.
            ICorrelater<char> correlater = 
                new LevenshteinCorrelater<char>(distanceCalculator, removalCost, insertionCost);
            
            string string1 = "ABC";
            string string2 = "ADC";

            // Correlate the strings.
            CorrelaterResult<char> result = correlater.Correlate(string1, string2);

            // Print some of the result
            Console.WriteLine(result.Distance);
            Console.WriteLine(result.BestMatch1);
            Console.WriteLine(result.BestMatch2);

            return result;
        }
    }
}
