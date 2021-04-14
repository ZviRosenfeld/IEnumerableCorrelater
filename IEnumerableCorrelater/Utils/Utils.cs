using IEnumerableCorrelater.Exceptions;
using IEnumerableCorrelater.Interfaces;
using System.Collections.Generic;

namespace IEnumerableCorrelater.Utils
{
    public static class Utils
    {
        /// <summary>
        /// Throws an exception if the collection contains a null element
        /// </summary>
        public static void CheckForNulls<T>(this ICollectionWrapper<T> collection, string collectionName)
        {
            for (int i = 0; i < collection.Length; i++)
                if (collection[i] == null || collection[i].Equals(default(T)))
                    throw new NullElementException(collectionName, i);
        }

        public static IEnumerable<T> GetNNullElemenets<T>(this int n)
        {
            for (var i = 0; i < n; i++)
                yield return default(T);
        }

        /// <summary>
        /// Returns the longest common subsequence (LCS) denoted by the CorrelaterResult
        /// </summary>
        public static IEnumerable<T> GetLcsFromResult<T>(this CorrelaterResult<T> result)
        {
            var lcs = new List<T>();
            int indexInCollection1 = 0, indexInCollection2 = 0;
            while (indexInCollection1 < result.BestMatch1.Length && indexInCollection2 < result.BestMatch2.Length)
            {
                var elementInCollection1 = result.BestMatch1[indexInCollection1];
                var elementInCollection2 = result.BestMatch2[indexInCollection2];

                if (elementInCollection1.Equals(default))
                    indexInCollection1++;
                else if (elementInCollection2.Equals(default))
                    indexInCollection2++;
                else
                {
                    if (elementInCollection1.Equals(elementInCollection2))
                        lcs.Add(elementInCollection1);
                    indexInCollection1++;
                    indexInCollection2++;
                }
            }

            return lcs;
        }
    }
}
