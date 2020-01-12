using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IEnumerableCorrelater.Interfaces;

namespace IEnumerableCorrelater.CollectionWrappers
{
    public class ArrayCollectionWrapper<T> : ICollectionWrapper<T>
    {
        private readonly T[] array;

        public ArrayCollectionWrapper(T[] array)
        {
            this.array = array;
        }

        public T this[int index] => array[index];

        public int Length => array.Length;

        public IEnumerator<T> GetEnumerator() => array.Cast<T>().GetEnumerator();

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var element in array)
                stringBuilder.Append(element + ", ");

            return stringBuilder.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
