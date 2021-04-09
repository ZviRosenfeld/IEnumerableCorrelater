using IEnumerableCorrelater.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;

namespace IEnumerableCorrelater.UnitTests.TestUtils
{
    class NeverEndingCorrelater<T> : ICorrelater<T>
    {
        public event Action<int, int> OnProgressUpdate;

        public CorrelaterResult<T> Correlate(IEnumerable<T> collection1, IEnumerable<T> collection2, CancellationToken cancellationToken = default)
        {
            while (true)
                cancellationToken.ThrowIfCancellationRequested();
        }
    }
}
