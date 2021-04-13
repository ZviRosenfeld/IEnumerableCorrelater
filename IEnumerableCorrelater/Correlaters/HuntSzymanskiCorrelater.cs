using IEnumerableCorrelater.Exceptions;
using IEnumerableCorrelater.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;

namespace IEnumerableCorrelater.Correlaters
{
    /// <summary>
    /// This class uses the Hunt–Szymanski algorithm to solve the LCS problem (thus finding the best match between 2 collections).
    /// </summary>
    public class HuntSzymanskiCorrelater<T> : AbstractCorrelater<T>
    {
        public override event Action<int, int> OnProgressUpdate;

        protected override CorrelaterResult<T> InternalCorrelate(ICollectionWrapper<T> collection1, ICollectionWrapper<T> collection2, CancellationToken cancellationToken = default)
        {
            var elementToLocationsInCollection2 = GetDictionaryFromElementToLocationsInCollection(collection2);

            var traceList = new TraceNode[collection1.Length + 1];

            var thresholds = new int[collection1.Length + 1];
            thresholds[0] = -1;
            for (var i = 1; i < thresholds.Length; i++)
                thresholds[i] = thresholds.Length + 1;

            for (var i = 0; i < collection1.Length; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                OnProgressUpdate?.Invoke(i + 1, collection1.Length);
                if (!elementToLocationsInCollection2.ContainsKey(collection1[i]))
                    continue;

                foreach (var j in elementToLocationsInCollection2[collection1[i]])
                {
                    var k = BinarySearchFindK(j, thresholds);
                    if (j < thresholds[k])
                    {
                        thresholds[k] = j;
                        traceList[k] = new TraceNode(i, j, k > 0 ? traceList[k - 1] : null);
                    }
                }
            }

            return GetResult(collection1, collection2, thresholds, traceList);
        }

        private Dictionary<T, List<int>> GetDictionaryFromElementToLocationsInCollection(ICollectionWrapper<T> collection)
        {
            var elementToLocationsInCollection = new Dictionary<T, List<int>>();
            for (var i = collection.Length - 1; i >= 0; i--)
            {
                if (!elementToLocationsInCollection.ContainsKey(collection[i]))
                    elementToLocationsInCollection[collection[i]] = new List<int>();
                elementToLocationsInCollection[collection[i]].Add(i);
            }

            return elementToLocationsInCollection;
        }

        /// <summary>
        /// Finds a number, k, in array thresholds so that (thresholds[i - 1] < j && j <= thresholds[i])
        /// </summary>
        private int BinarySearchFindK(int j, int[] thresholds)
        {
            int start = 0, end = thresholds.Length - 1;
            while (start < end)
            {
                var middle = (start + end) / 2;
                if (thresholds[middle - 1] < j && j <= thresholds[middle])
                    return middle;
                if (start + 1 == end)
                    return end;
                if (j > thresholds[middle])
                    start = middle;
                else
                    end = middle;
            }

            throw new InternalException($"Code 1005 (reached the end of {nameof(HuntSzymanskiCorrelater<T>)}.{nameof(BinarySearchFindK)})");
        }

        private CorrelaterResult<T> GetResult(ICollectionWrapper<T> collection1, ICollectionWrapper<T> collection2, int[] thresholds, TraceNode[] traceList)
        {
            var longestSubsequentSize = GetLongestSubsequentSize(thresholds);
            var distance = collection1.Length + collection2.Length - 2 * longestSubsequentSize;

            var (bestMatch1, bestMatch2) = GetBestMatches(collection1, collection2, longestSubsequentSize, traceList);
            return new CorrelaterResult<T>(distance, bestMatch1, bestMatch2);
        }

        private int GetLongestSubsequentSize(int[] thresholds)
        {
            for (var k = thresholds.Length - 1; k >= 0; k--)
                if (thresholds[k] < thresholds.Length + 1)
                    return k;

            throw new InternalException($"Code 1004 (reached the end of {nameof(HuntSzymanskiCorrelater<T>)}.{nameof(GetLongestSubsequentSize)})");
        }

        private (T[], T[]) GetBestMatches(ICollectionWrapper<T> collection1, ICollectionWrapper<T> collection2, int longestSubsequentSize, TraceNode[] traceList)
        {
            var bestMatch1 = new List<T>();
            var bestMatch2 = new List<T>();

            var traceNode = traceList[longestSubsequentSize];
            var locationInCollection1 = collection1.Length - 1;
            var locationInCollection2 = collection2.Length - 1;
            while (traceNode != null)
            {
                AddElementsFromCollection(collection1, bestMatch1, bestMatch2, locationInCollection1, traceNode.IndexInCollection1);
                AddElementsFromCollection(collection2, bestMatch2, bestMatch1, locationInCollection2, traceNode.IndexInCollection2);

                bestMatch1.Add(collection1[traceNode.IndexInCollection1]);
                bestMatch2.Add(collection2[traceNode.IndexInCollection2]);

                locationInCollection1 = traceNode.IndexInCollection1 - 1;
                locationInCollection2 = traceNode.IndexInCollection2 - 1;
                traceNode = traceNode.Previous;
            }
            AddElementsFromCollection(collection1, bestMatch1, bestMatch2, locationInCollection1, -1);
            AddElementsFromCollection(collection2, bestMatch2, bestMatch1, locationInCollection2, -1);


            bestMatch1.Reverse();
            bestMatch2.Reverse();

            return (bestMatch1.ToArray(), bestMatch2.ToArray());
        }

        /// <summary>
        /// Adds the elements from "collection" between the startIndex to the endIndex (descending) into bestMatch1 list. bestMatch2 gets nulls.
        /// </summary>
        private void AddElementsFromCollection(ICollectionWrapper<T> collection, List<T> bestMatch1, List<T> bestMatch2, int startIndex, int endIndex)
        {
            for (; startIndex > endIndex; startIndex--)
            {
                bestMatch1.Add(collection[startIndex]);
                bestMatch2.Add(default);
            }
        }

        private class TraceNode
        {
            public int IndexInCollection1 { get; }

            public int IndexInCollection2 { get; }

            public TraceNode Previous { get; }

            public TraceNode(int indexInCollection1, int indexInCollection2, TraceNode previous)
            {
                IndexInCollection1 = indexInCollection1;
                IndexInCollection2 = indexInCollection2;
                Previous = previous;
            }

            public override string ToString() => $"Index ({IndexInCollection1}, {IndexInCollection2})";
        }
    }
}
