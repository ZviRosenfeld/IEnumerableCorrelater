using System.Collections;
using System.Collections.Generic;
using System.Text;
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
        
        public IEnumerator<T> GetEnumerator() => list.GetEnumerator();

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var element in list)
                stringBuilder.Append(element + ", ");

            return stringBuilder.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
