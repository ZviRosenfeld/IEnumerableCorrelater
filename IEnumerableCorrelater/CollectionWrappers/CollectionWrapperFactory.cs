using System.Collections.Generic;
using System.Linq;
using IEnumerableCorrelater.Interfaces;

namespace IEnumerableCorrelater.CollectionWrappers
{
    static class CollectionWrapperFactory
    {
        public static ICollectionWrapper<T> ToCollectionWrapper<T>(this IEnumerable<T> collection)
        {
            switch (collection)
            {
                case T[] array: return new ArrayCollectionWrapper<T>(array);
                case IList<T> list: return new ListCollectionWrapper<T>(list);
                default: return new ArrayCollectionWrapper<T>(collection.ToArray());
            }
        }
    }
}
