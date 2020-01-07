using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IEnumerableCorrelater.Interfaces;

namespace IEnumerableCorrelater.CollectionWrappers
{
    class CollectionWrapperFactory
    {
        public ICollectionWrapper<T> GetCollectionWrapper<T>(IEnumerable<T> collection)
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
