using System.Collections.Generic;

namespace IEnumerableCompare.Interfaces
{
    public interface IEnumerableComparer<T>
    {
        CompareResult<T> Compare(IEnumerable<T> enumerable1, IEnumerable<T> enumerable2);

        CompareResult<T> Compare(T[] array1, T[] array2);
    }
}
