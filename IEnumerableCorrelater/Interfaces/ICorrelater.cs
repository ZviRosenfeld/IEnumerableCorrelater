using System;
using System.Collections.Generic;

namespace IEnumerableCorrelater.Interfaces
{
    public interface ICorrelater<T>
    {
        CorrelaterResult<T> Correlate(IEnumerable<T> collection1, IEnumerable<T> collection2);
        
        event Action<int, int> OnProgressUpdate;
    }
}