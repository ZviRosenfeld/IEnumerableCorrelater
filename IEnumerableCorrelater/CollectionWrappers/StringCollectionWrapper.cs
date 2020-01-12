using System.Collections;
using System.Collections.Generic;
using System.Text;
using IEnumerableCorrelater.Interfaces;

namespace IEnumerableCorrelater.CollectionWrappers
{
    public class StringCollectionWrapper : ICollectionWrapper<char>
    {
        private readonly string s;

        public StringCollectionWrapper(string s)
        {
            this.s = s;
        }

        public char this[int index] => s[index];

        public int Length => s.Length;

        public IEnumerator<char> GetEnumerator() => s.GetEnumerator();

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
