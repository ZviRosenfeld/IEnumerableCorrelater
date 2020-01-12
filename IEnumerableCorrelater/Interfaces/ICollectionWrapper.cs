using System.Collections.Generic;

namespace IEnumerableCorrelater.Interfaces
{
    /// <summary>
    /// Wrappers any collection, and provides indexing
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICollectionWrapper<T> : IEnumerable<T>
    {
        T this[int index] { get; }

        int Length { get; }
    }
}
