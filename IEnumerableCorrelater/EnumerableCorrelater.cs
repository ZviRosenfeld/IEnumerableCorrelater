using System.Collections.Generic;
using IEnumerableCorrelater.CollectionWrappers;
using IEnumerableCorrelater.Interfaces;

namespace IEnumerableCorrelater
{
    /// <summary>
    /// This class used dynamic programming to calculate the Levenshtein distance between two collections.
    /// </summary>
    public class EnumerableCorrelater<T>
    {
        private readonly ICorrelater<T> correlater;

        /// <summary>
        /// This class used dynamic programming to calculate the Levenshtein distance between two collections.
        /// </summary>
        public EnumerableCorrelater(ICorrelater<T> correlater)
        {
            this.correlater = correlater;
        }

        public CorrelaterResult<T> Correlate(IEnumerable<T> enumerable1, IEnumerable<T> enumerable2)
        {
            var factory = new CollectionWrapperFactory();
            return correlater.Compare(factory.GetCollectionWrapper(enumerable1), factory.GetCollectionWrapper(enumerable2));
        }
    }
}
