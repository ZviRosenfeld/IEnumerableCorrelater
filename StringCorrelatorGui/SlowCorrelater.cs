using System;
using System.Collections.Generic;
using System.Threading;
using IEnumerableCorrelater;
using IEnumerableCorrelater.Interfaces;

namespace StringCorrelatorGui
{
    class SlowCorrelater<T> : ICorrelater<T>
    {
        private readonly ICorrelater<T> innerCorrelater;
        private readonly int sleepTime;

        public SlowCorrelater(ICorrelater<T> innerCorrelater, int sleepTime)
        {
            this.innerCorrelater = innerCorrelater;
            innerCorrelater.OnProgressUpdate += (p, t) => OnProgressUpdate?.Invoke(t, p);
            this.sleepTime = sleepTime;
        }

        public CorrelaterResult<T> Correlate(IEnumerable<T> collection1, IEnumerable<T> collection2, CancellationToken cancellationToken = default)
        {
            Thread.Sleep(sleepTime);
            return innerCorrelater.Correlate(collection1, collection2);
        }

        public event Action<int, int> OnProgressUpdate;
    }
}
