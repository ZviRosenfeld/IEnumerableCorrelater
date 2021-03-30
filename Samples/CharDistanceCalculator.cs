using System;
using System.Collections.Generic;
using IEnumerableCorrelater.Interfaces;

namespace Samples
{
    /// <summary>
    /// An implantation of an IDistanceCalculator&lt;char&gt;
    /// </summary>
    class CharDistanceCalculator : IDistanceCalculator<char>
    {
        private const int DEFAULT_DISTANCE = 20;
        // We'll use a dictionary that will hold the distances between different pairs
        private readonly Dictionary<Tuple<char, char>, uint> distance = new Dictionary<Tuple<char, char>, uint>()
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

        public uint Distance(char element1, char element2)
        {
            // If the elements are equal, they should return a distance of 0
            if (element1.Equals(element2))
                return 0;

            var tuple = new Tuple<char, char>(element1, element2);
            if (distance.ContainsKey(tuple))
                return distance[tuple];

            // For any pairs not in the dictionary, we'll return a default distance.
            return DEFAULT_DISTANCE;
        }
    }
}
