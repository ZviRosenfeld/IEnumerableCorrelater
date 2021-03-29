using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IEnumerableCorrelater.Exceptions;
using IEnumerableCorrelater.Interfaces;

namespace IEnumerableCorrelater.CollectionWrappers
{
    public class StringCollectionWrapper<T> : ICollectionWrapper<T>
    {
        private readonly string s;

        public StringCollectionWrapper(string s)
        {
            if (typeof(T) != typeof(char))
                throw new InternalException($"code 1002 (the generic type of {nameof(StringCollectionWrapper<T>)} is {typeof(T)})");

            this.s = s;
        }

        public T this[int index] => (T)(object)s[index];

        public int Length => s.Length;

        public IEnumerator<T> GetEnumerator() => s.Cast<T>().GetEnumerator();

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var c in s)
                stringBuilder.Append(c + ", ");

            return stringBuilder.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
