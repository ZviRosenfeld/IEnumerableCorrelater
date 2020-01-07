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
    }
}
