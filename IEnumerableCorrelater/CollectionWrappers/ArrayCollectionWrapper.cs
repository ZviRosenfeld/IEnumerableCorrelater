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

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var element in array)
                stringBuilder.Append(element + ", ");

            return stringBuilder.ToString();
        }
    }
}
