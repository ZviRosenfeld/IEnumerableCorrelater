using IEnumerableCorrelater.CollectionWrappers;
using IEnumerableCorrelater.Exceptions;
using IEnumerableCorrelater.Interfaces;
using IEnumerableCorrelater.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IEnumerableCorrelater.Correlaters
{
    public abstract class AbstractCorrelater<T> : ICorrelater<T>
    {
        public abstract event Action<int, int> OnProgressUpdate;

        public AbstractCorrelater()
        {
            if (default(T) != null && typeof(T) != typeof(char))
                throw new EnumerableCorrelaterException($"{nameof(T)} must be nullable or a char");
        }

        public CorrelaterResult<T> Correlate(IEnumerable<T> collection1, IEnumerable<T> collection2, CancellationToken cancellationToken = default)
        {
            if (!collection1.Any() && !collection2.Any())
               return new CorrelaterResult<T>(0, new T[0], new T[0]);
            
            var collection1Wrapper = collection1.ToCollectionWrapper();
            var collection2Wrapper = collection2.ToCollectionWrapper();

            collection1Wrapper.CheckForNulls(nameof(collection1));
            collection2Wrapper.CheckForNulls(nameof(collection2));

            return InternalCorrelate(collection1Wrapper, collection2Wrapper, cancellationToken);
        }

        protected abstract CorrelaterResult<T> InternalCorrelate(ICollectionWrapper<T> collection1, ICollectionWrapper<T> collection2, CancellationToken cancellationToken = default);
    }
}
