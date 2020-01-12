using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IEnumerableCorrelater.CollectionWrappers;
using IEnumerableCorrelater.Interfaces;

namespace IEnumerableCorrelater.CorrelaterWrappers
{
    /// <summary>
    /// SplitToChunksCorrelaterWrapper wraps a correlater.
    /// It splits the collection or string into chunks, and correlates each chunk individually.
    /// When it's done, it correlates the edges of each chunk to combine them.
    /// </summary>
    public class SplitToChunksCorrelaterWrapper<T> : ICorrelater<T>
    {
        private readonly CollectionWrapperFactory factory = new CollectionWrapperFactory();
        private readonly ICorrelater<T> innerCorrelater;
        private readonly int chunkSize;
        private readonly int maxEdgeSize;

        public SplitToChunksCorrelaterWrapper(ICorrelater<T> innerCorrelater, int chunkSize)
        {
            this.chunkSize = chunkSize;
            this.innerCorrelater = innerCorrelater;
            maxEdgeSize = chunkSize / 2;
        }

        public CorrelaterResult<T> Compare(ICollectionWrapper<T> collection1, ICollectionWrapper<T> collection2)
        {
            var results = Map(collection1, collection2);
            return Reduce(results);
        }

        private List<Task<CorrelaterResult<T>>> Map(ICollectionWrapper<T> collection1, ICollectionWrapper<T> collection2)
        {
            var resultTasks = new List<Task<CorrelaterResult<T>>>();
            for (int i = 0; i < Math.Max(collection1.Length, collection2.Length); i = i + chunkSize)
            {
                var wrappedCollection1 = new OffsetCollectionWrapper<T>(collection1, Math.Min(collection1.Length, i), Math.Min(collection1.Length, i + chunkSize));
                var wrappedCollection2 = new OffsetCollectionWrapper<T>(collection2, Math.Min(collection2.Length, i), Math.Min(collection2.Length, i + chunkSize));
                resultTasks.Add(Task.Run(() => innerCorrelater.Compare(wrappedCollection1, wrappedCollection2)));
            }
            
            return resultTasks;
        }
        
        /// <summary>
        /// When connecting section A with section B, the Reduce method tries to better connect the "edges" of A and B.
        /// </summary>
        private CorrelaterResult<T> Reduce(List<Task<CorrelaterResult<T>>> resultTasks)
        {
            var list1 = new List<T>();
            var list2 = new List<T>();
            var distance = 0;
            var endEdgeIndex = -1;

            for (var i = 0; i < resultTasks.Count; i++)
            {
                var result = resultTasks[i].Result;
                var startEdgeIndex = i > 0 ? GetStartEdgeIndex(resultTasks[i].Result) : 0;
                if (i > 0)
                    AddEdge(endEdgeIndex, startEdgeIndex, result, resultTasks[i - 1].Result, list1, list2);
                endEdgeIndex = i < resultTasks.Count - 1 ? GetEndEdgeIndex(result) : result.BestMatch1.Length;

                distance += result.Distance;
                AddRange(result.BestMatch1, list1, startEdgeIndex, endEdgeIndex);
                AddRange(result.BestMatch2, list2, startEdgeIndex, endEdgeIndex);
            }

            return new CorrelaterResult<T>(distance, list1.ToArray(), list2.ToArray());
        }

        /// <summary>
        /// A certain stratch of the two chunks - which includes the end of the previous chunk, and the begining of the current chunk, is considered the chunks "edge".
        /// We'll correlate this edge separately, to prevent loosing matches between chunks.  
        /// </summary>
        private void AddEdge(int startFromIndexOnPreviousResult , int goUpToIndexOnCurrentResult, CorrelaterResult<T> currentResult, CorrelaterResult<T> previousResult, List<T> list1, List<T> list2)
        {
            var collection1 = GetEdgeCollection(previousResult.BestMatch1, currentResult.BestMatch1,
                startFromIndexOnPreviousResult, goUpToIndexOnCurrentResult);
            var collection2 = GetEdgeCollection(previousResult.BestMatch2, currentResult.BestMatch2,
                startFromIndexOnPreviousResult, goUpToIndexOnCurrentResult);

            var result = innerCorrelater.Compare(factory.GetCollectionWrapper(collection1),
                factory.GetCollectionWrapper(collection2));

            AddRange(result.BestMatch1, list1, 0, result.BestMatch1.Length);
            AddRange(result.BestMatch2, list2, 0, result.BestMatch2.Length);
        }

        private IEnumerable<T> GetEdgeCollection(T[] firstPart, T[] secondPart, int start, int end) =>
            firstPart.Skip(start).Concat(secondPart.Take(end)).Where(e => !e.Equals(default(T)));


        /// <summary>
        /// Finds the first first index, i, so that for any index j where j > i T[j] == null
        /// </summary>
        private int GetEndEdgeIndex(CorrelaterResult<T> result) =>
            Math.Min(GetEndEdgeIndex(result.BestMatch1), GetEndEdgeIndex(result.BestMatch2));

        /// <summary>
        /// Finds the first first index, i, so that for any index j where j > i T[j] == null
        /// </summary>
        private int GetEndEdgeIndex(T[] array)
        {
            int i;
            for (i = array.Length - 1; i >= 0 && i >= array.Length - maxEdgeSize; i--)
                if (!array[i].Equals(default(T)))
                    return i + 1;

            return i + 1;
        }

        /// <summary>
        /// Finds the last index, i, so that for any index j where i > j T[j] == null
        /// </summary>
        private int GetStartEdgeIndex(CorrelaterResult<T> result) =>
            Math.Max(GetStartEdgeIndex(result.BestMatch1), GetStartEdgeIndex(result.BestMatch2));


        /// <summary>
        /// Finds the last index, i, so that for any index j where i > j T[j] == null
        /// </summary>
        private int GetStartEdgeIndex(T[] array)
        {
            int i;
            for (i = 0; i < array.Length && i < maxEdgeSize; i++)
                if (!array[i].Equals(default(T)))
                    return i;

            return i;
        }

        private void AddRange(T[] from, List<T> to, int fromIndex, int toIndex)
        {
            for (int i = fromIndex; i < toIndex; i++)
                to.Add(from[i]);
        }
    }
}
