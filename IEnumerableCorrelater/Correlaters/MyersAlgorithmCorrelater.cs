using IEnumerableCorrelater.CollectionWrappers;
using IEnumerableCorrelater.Exceptions;
using IEnumerableCorrelater.Interfaces;
using IEnumerableCorrelater.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IEnumerableCorrelater.Correlaters
{
    /// <summary>
    /// Uses Myer's algorithm to find the diffs between 2 IEnumerables.
    /// This is a greedy algorithm that’s fast and produces diffs that tend to be of good quality most of the time.
    /// </summary>
    public class MyersAlgorithmCorrelater<T> : ICorrelater<T>
    {
        public event Action<int, int> OnProgressUpdate;

        public CorrelaterResult<T> Correlate(IEnumerable<T> collection1, IEnumerable<T> collection2)
        {
            var collection1Wrapper = collection1.ToCollectionWrapper();
            var collection2Wrapper = collection2.ToCollectionWrapper();

            collection1Wrapper.CheckForNulls(nameof(collection1));
            collection2Wrapper.CheckForNulls(nameof(collection2));

            var trace = CreateTraceTable(collection1Wrapper, collection2Wrapper);

            return CreateResult(trace, collection1Wrapper, collection2Wrapper);
        }

        private List<NegativeArray<int>> CreateTraceTable(ICollectionWrapper<T> collection1Wrapper, ICollectionWrapper<T> collection2Wrapper)
        {
            var maxDistance = collection1Wrapper.Length + collection2Wrapper.Length;
            var trace = new List<NegativeArray<int>>();

            for (var d = 0; d <= maxDistance; d++)
            {
                trace.Add(new NegativeArray<int>(maxDistance * 2));
                for (var k = -1 * d; k <= d; k += 2)
                {
                    int x;
                    if (d == 0)
                        x = 0;
                    else if (k == -d || (k != d && trace[d - 1][k - 1] < trace[d - 1][k + 1]))
                        x = trace[d - 1][k + 1];
                    else
                        x = trace[d - 1][k - 1] + 1;

                    var y = x - k;

                    while (x < collection1Wrapper.Length && y < collection2Wrapper.Length && collection1Wrapper[x].Equals(collection2Wrapper[y]))
                    {
                        x = x + 1;
                        y = y + 1;
                    }

                    trace[d][k] = x;

                    if (x == collection1Wrapper.Length && y == collection2Wrapper.Length)
                        return trace;
                }
            }

            throw new InternalException($"Code 1003 (reached the end of {nameof(MyersAlgorithmCorrelater<T>)}.{nameof(Correlate)})");
        }

        private CorrelaterResult<T> CreateResult(List<NegativeArray<int>> trace, ICollectionWrapper<T> collection1Wrapper, ICollectionWrapper<T> collection2Wrapper)
        {
            int x = collection1Wrapper.Length - 1, y = collection2Wrapper.Length - 1;
            var bestMatch1 = new LinkedList<T>();
            var bestMatch2 = new LinkedList<T>();

            for (var d = trace.Count - 1; d >= 0; d--)
            {
                var k = x - y;
                int prev_k = 0, prev_x = 0, prev_y = 0;
                if (d > 0)
                {
                    if (k == -d || (k != d && trace[d - 1][k - 1] < trace[d - 1][k + 1]))
                        prev_k = k + 1;
                    else
                        prev_k = k - 1;

                    prev_x = trace[d - 1][prev_k];
                    prev_y = prev_x - prev_k;
                }
                else
                {
                    prev_x = 0;
                    prev_y = 0;
                }

                while (x >= prev_x && y >= prev_y)
                {
                    bestMatch1.AddFirst(collection1Wrapper[x]);
                    bestMatch2.AddFirst(collection2Wrapper[y]);
                    x = x - 1;
                    y = y - 1;
                }

                if (d > 0)
                {
                    if (x == prev_x)
                    {
                        bestMatch1.AddFirst(collection1Wrapper[x]);
                        bestMatch2.AddFirst(default(T));
                        x = x - 1;
                    }
                    else
                    {
                        bestMatch1.AddFirst(default(T));
                        bestMatch2.AddFirst(collection2Wrapper[y]);
                        y = y - 1;
                    }
                }
            }

            return new CorrelaterResult<T>(trace.Count - 1, bestMatch1.ToArray(), bestMatch2.ToArray());
        }
    }
}
