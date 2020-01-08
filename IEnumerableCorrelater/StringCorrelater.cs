using System.Threading;
using IEnumerableCorrelater.CollectionWrappers;
using IEnumerableCorrelater.Interfaces;

namespace IEnumerableCorrelater
{
    public class StringCorrelater
    {
        private readonly ICorrelater<char> correlater;

        /// <summary>
        /// This class used dynamic programming to calculate the Levenshtein distance between two collections.
        /// </summary>
        public StringCorrelater(ICorrelater<char> correlater)
        {
            this.correlater = correlater;
        }

        public CorrelaterResult<char> Correlate(string string1, string string2) =>
            correlater.Compare(new StringCollectionWrapper(string1), new StringCollectionWrapper(string2));
    }
}
