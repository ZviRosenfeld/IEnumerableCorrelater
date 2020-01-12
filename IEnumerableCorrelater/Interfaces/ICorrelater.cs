using System;

namespace IEnumerableCorrelater.Interfaces
{
    public interface ICorrelater<T>
    {
        CorrelaterResult<T> Compare(ICollectionWrapper<T> collection1, ICollectionWrapper<T> collection2);
        
        event Action<int, int> OnProgressUpdate;
    }
}