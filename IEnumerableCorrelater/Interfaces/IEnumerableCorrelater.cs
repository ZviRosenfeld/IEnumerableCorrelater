using System.Collections.Generic;

namespace IEnumerableCorrelater.Interfaces
{
    public interface IEnumerableCorrelater<T>
    {
        CorrelaterResult<T> Correlate(IEnumerable<T> enumerable1, IEnumerable<T> enumerable2);  
    }
}
