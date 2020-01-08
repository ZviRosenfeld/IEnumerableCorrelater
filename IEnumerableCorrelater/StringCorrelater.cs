using IEnumerableCorrelater.CollectionWrappers;
using IEnumerableCorrelater.Interfaces;

namespace IEnumerableCorrelater
{
    /// <summary>
    /// Use a StringCorrelater to compare strings
    /// </summary>
    public class StringCorrelater
    {
        private readonly ICorrelater<char> correlater;

        /// <summary>
        /// Use a StringCorrelater to compare strings
        /// </summary>
        public StringCorrelater(ICorrelater<char> correlater)
        {
            this.correlater = correlater;
        }

        public CorrelaterResult<char> Correlate(string string1, string string2) =>
            correlater.Compare(new StringCollectionWrapper(string1), new StringCollectionWrapper(string2));
    }
}
