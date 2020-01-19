using IEnumerableCorrelater.Exceptions;
using IEnumerableCorrelater.Interfaces;

namespace IEnumerableCorrelater
{
    static class Utils
    {
        /// <summary>
        /// Throws an exception if the collection contains a null element
        /// </summary>
        public static void CheckFullNulls<T>(this ICollectionWrapper<T> collection, string collectionName)
        {
            for (int i = 0; i < collection.Length; i++)
                if (collection[i] == null || collection[i].Equals(default(T)))
                    throw new NullElementException(collectionName, i);
        }
    }
}
