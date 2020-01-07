using System.Collections.Generic;

namespace IEnumerableCorrelater.Interfaces
{
    public interface IEnumerableCorrelater<T>
    {
        CorrelaterResult<T> Compare(IEnumerable<T> enumerable1, IEnumerable<T> enumerable2);

        CorrelaterResult<T> Compare(T[] array1, T[] array2);
    }
}
