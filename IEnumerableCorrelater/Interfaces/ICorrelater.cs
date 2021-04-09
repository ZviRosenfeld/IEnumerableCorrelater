using System;
using System.Collections.Generic;
using System.Threading;

namespace IEnumerableCorrelater.Interfaces
{
    public interface ICorrelater<T>
    {
        CorrelaterResult<T> Correlate(IEnumerable<T> collection1, IEnumerable<T> collection2, CancellationToken cancellationToken = default);
        
        event Action<int, int> OnProgressUpdate;
    }
}