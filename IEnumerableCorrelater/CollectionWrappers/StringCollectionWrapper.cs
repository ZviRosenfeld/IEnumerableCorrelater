using System.Text;
using IEnumerableCorrelater.Interfaces;

namespace IEnumerableCorrelater.CollectionWrappers
{
    class StringCollectionWrapper : ICollectionWrapper<char>
    {
        private readonly string s;

        public StringCollectionWrapper(string s)
        {
            this.s = s;
        }

        public char this[int index] => s[index];

        public int Length => s.Length;

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var c in s)
                stringBuilder.Append(c + ", ");

            return stringBuilder.ToString();
        }
    }
}
