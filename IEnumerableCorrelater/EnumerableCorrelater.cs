using System.Collections.Generic;
using IEnumerableCorrelater.CollectionWrappers;
using IEnumerableCorrelater.Interfaces;

namespace IEnumerableCorrelater
{
    /// <summary>
    /// Use an EnumerableCorrelater&lt;T&gt; to compare collections 
    /// </summary>
    public class EnumerableCorrelater<T>
    {
        private readonly ICorrelater<T> correlater;

        /// <summary>
        /// Use an EnumerableCorrelater&lt;T&gt; to compare collections 
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
