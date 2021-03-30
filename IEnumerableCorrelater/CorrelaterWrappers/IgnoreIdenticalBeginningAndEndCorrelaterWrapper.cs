using IEnumerableCorrelater.CollectionWrappers;
using IEnumerableCorrelater.Interfaces;
using System;
using System.Collections.Generic;

namespace IEnumerableCorrelater.CorrelaterWrappers
{
    /// <summary>
    /// This class wraps a correlater.
    /// It improves the correlation's performance be removing the beginning and the end of the sequence if they are equal.
    /// This can be useful in cases like source code correlation - where the changes are likely only a few lines in the middle of a file.
    /// </summary>
    public class IgnoreIdenticalBeginningAndEndCorrelaterWrapper<T> : ICorrelater<T>
    {
        private readonly ICorrelater<T> innerCorrelater;

        public IgnoreIdenticalBeginningAndEndCorrelaterWrapper(ICorrelater<T> innerCorrelater)
        {
            this.innerCorrelater = innerCorrelater;
            innerCorrelater.OnProgressUpdate += (p, t) => OnProgressUpdate?.Invoke(p, t);
        }

        public event Action<int, int> OnProgressUpdate;

        public CorrelaterResult<T> Correlate(IEnumerable<T> collection1, IEnumerable<T> collection2)
        {
            var collection1Wrapper = collection1.ToCollectionWrapper();
            var collection2Wrapper = collection2.ToCollectionWrapper();

            var startIndex = GetFirstNotEqualIndex(collection1Wrapper, collection2Wrapper);
            var endIndexes = GetLastNotEqualIndexes(collection1Wrapper, collection2Wrapper, startIndex);

            var innerCorrelaterResult = innerCorrelater.Correlate(
                new OffsetCollectionWrapper<T>(collection1Wrapper, startIndex, endIndexes.Item1),
                new OffsetCollectionWrapper<T>(collection2Wrapper, startIndex, endIndexes.Item2));

            return CreateResult(collection1Wrapper, collection2Wrapper, startIndex, endIndexes, innerCorrelaterResult);
        }

        private static CorrelaterResult<T> CreateResult(ICollectionWrapper<T> collection1Wrapper, ICollectionWrapper<T> collection2Wrapper, int startIndex, (int, int) endIndexes, CorrelaterResult<T> innerCorrelaterResult)
        {
            var endPartSize = collection1Wrapper.Length - endIndexes.Item1;
            var totalLength = startIndex + endPartSize + innerCorrelaterResult.BestMatch1.Length;
            var bastMatch1 = new T[totalLength];
            var bastMatch2 = new T[totalLength];
            var index = 0;
            for (; index < startIndex; index++)
            {
                bastMatch1[index] = collection1Wrapper[index];
                bastMatch2[index] = collection2Wrapper[index];
            }
            for (var i = 0; i < innerCorrelaterResult.BestMatch1.Length; index++, i++)
            {
                bastMatch1[index] = innerCorrelaterResult.BestMatch1[i];
                bastMatch2[index] = innerCorrelaterResult.BestMatch2[i];
            }
            for (var i = endIndexes.Item1; i < collection1Wrapper.Length; index++, i++)
            {
                bastMatch1[index] = collection1Wrapper[i];
                bastMatch2[index] = collection1Wrapper[i];
            }
            return new CorrelaterResult<T>(innerCorrelaterResult.Distance, bastMatch1, bastMatch2);
        }

        private int GetFirstNotEqualIndex(ICollectionWrapper<T> collection1, ICollectionWrapper<T> collection2)
        {
            int startIndex = 0;
            int maxLength = Math.Min(collection1.Length, collection2.Length);
            for (; startIndex < maxLength && collection1[startIndex].Equals(collection2[startIndex]); startIndex++) ;
            return startIndex;
        }

        private (int, int) GetLastNotEqualIndexes(ICollectionWrapper<T> collection1, ICollectionWrapper<T> collection2, int startIndex)
        {
            var endIndex1 = collection1.Length;
            var endIndex2 = collection2.Length;
            for (; endIndex1 > startIndex && endIndex2 > startIndex && collection1[endIndex1 - 1].Equals(collection2[endIndex2 - 1]); endIndex1--, endIndex2--) ;
            return (endIndex1, endIndex2);
        }
    }
}
