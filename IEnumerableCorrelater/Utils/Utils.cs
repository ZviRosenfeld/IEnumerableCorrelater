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
    }
}
