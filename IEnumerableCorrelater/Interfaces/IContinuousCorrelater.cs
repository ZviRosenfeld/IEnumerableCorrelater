using System;

namespace IEnumerableCorrelater.Interfaces
{
    /// <summary>
    /// Correlation of big collections can take a considerable amount of time.
    /// 
    /// ContinuousCorrelaters solve this problem by providing the caller with updates
    /// on the correlation of the earlier segments of the collection while they continue working out the correlation of the later ones. 
    /// </summary>
    public interface IContinuousCorrelater<T> : ICorrelater<T>
    {
        /// <summary>
        /// OnProgressUpdate will only contain the new segment (and not previously sent segments).
        /// Please note that there's no guarantee that the accumulated distance sent to the OnResultUpdate will equal the actual distance.
        /// </summary>
        event Action<CorrelaterResult<T>> OnResultUpdate;
    }
}
