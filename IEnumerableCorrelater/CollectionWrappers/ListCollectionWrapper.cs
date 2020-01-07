using System.Collections.Generic;
using IEnumerableCorrelater.Interfaces;

namespace IEnumerableCorrelater.CollectionWrappers
{
    class ListCollectionWrapper<T> : ICollectionWrapper<T>
    {
        private readonly IList<T> list;

        public ListCollectionWrapper(IList<T> list)
        {
            this.list = list;
        }

        public T this[int index] => list[index];

        public int Length => list.Count;
    }
}
