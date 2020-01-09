using System;
using System.Collections.Generic;
using IEnumerableCorrelater.Interfaces;

namespace Samples
{
    /// <summary>
    /// An implimantation of an IDistanceCalculator&lt;char&gt;
    /// </summary>
    class CharDistanceCalculator : IDistanceCalculator<char>
    {
        private const int DEFAULT_DISTANCE = 20;
        // We'll use a dictionary that will hold the distances between diffrent pairs
        private readonly Dictionary<Tuple<char, char>, int> distance = new Dictionary<Tuple<char, char>, int>()
        {
            {new Tuple<char, char>('a', 'e'), 1 },
            {new Tuple<char, char>('a', 'i'), 2 },
            {new Tuple<char, char>('a', 'o'), 2 },
            {new Tuple<char, char>('a', 'u'), 3 },
            {new Tuple<char, char>('b', 'd'), 1 },
            {new Tuple<char, char>('c', 'e'), 2 },
            {new Tuple<char, char>('c', 's'), 2 },
            {new Tuple<char, char>('c', 'i'), 3 },
        }; 

        public int Distance(char element1, char element2)
        {
            if (element1.Equals(element2))
                return 0;

            var tuple = new Tuple<char, char>(element1, element2);
            if (distance.ContainsKey(tuple))
                return distance[tuple];

            // For any distances not in the dictinary, we'll return a default distance.
            return DEFAULT_DISTANCE;
        }
    }
}
