using IEnumerableCorrelater.CorrelaterWrappers;
using IEnumerableCorrelater.Exceptions;
using IEnumerableCorrelater.Interfaces;
using System;
using System.Threading;

namespace IEnumerableCorrelater.Correlaters
{
    /// <summary>
    /// PatienceDiffCorrelater is an algorithm that was developed specifically for comparing diffs in code.
    /// It does a very good job at creating human-readable diffs.
    /// </summary>
    public class PatienceDiffCorrelater<T> : AbstractCorrelater<T>, IContinuousCorrelater<T>
    {
        private readonly IContinuousCorrelater<T> innerCorrelater = 
            new IgnoreIdenticalBeginningAndEndCorrelaterWrapper<T>(
                new SplitByPatienceAlgorithmWrapper<T>(
                     new IgnoreIdenticalBeginningAndEndCorrelaterWrapper<T>(
                        new MyersAlgorithmCorrelater<T>())));

        public PatienceDiffCorrelater()
        {
            if (default(T) != null && typeof(T) != typeof(char))
                throw new EnumerableCorrelaterException($"{nameof(T)} must be nullable or a char");

            innerCorrelater.OnProgressUpdate += (p, t) => OnProgressUpdate?.Invoke(p, t);
            innerCorrelater.OnResultUpdate += r => OnResultUpdate?.Invoke(r);
        }

        public override event Action<int, int> OnProgressUpdate;
        public event Action<CorrelaterResult<T>> OnResultUpdate;

        protected override CorrelaterResult<T> InternalCorrelate(ICollectionWrapper<T> collection1, ICollectionWrapper<T> collection2, CancellationToken cancellationToken = default)
        {
            return innerCorrelater.Correlate(collection1, collection2, cancellationToken);
        }
    }
}
