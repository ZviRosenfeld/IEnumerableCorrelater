﻿using IEnumerableCorrelater.Exceptions;
using IEnumerableCorrelater.Interfaces;
using IEnumerableCorrelater.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IEnumerableCorrelater.Correlaters
{
    /// <summary>
    /// Uses Myer's algorithm to find the diffs between 2 IEnumerables.
    /// This is a greedy algorithm that’s fast and produces diffs that tend to be of good quality most of the time.
    /// </summary>
    public class MyersAlgorithmCorrelater<T> : AbstractCorrelater<T>
    {
        public override event Action<int, int> OnProgressUpdate;

        protected override CorrelaterResult<T> InternalCorrelate(ICollectionWrapper<T> collection1, ICollectionWrapper<T> collection2, CancellationToken cancellationToken = default)
        {
            var trace = CreateTraceTable(collection1, collection2, cancellationToken);
            return CreateResult(trace, collection1, collection2);
        }

        private List<NegativeArray<int>> CreateTraceTable(ICollectionWrapper<T> collection1Wrapper, ICollectionWrapper<T> collection2Wrapper, CancellationToken cancellationToken)
        {
            var maxDistance = collection1Wrapper.Length + collection2Wrapper.Length;
            var trace = new List<NegativeArray<int>>();

            for (var distance = 0; distance <= maxDistance; distance++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                trace.Add(new NegativeArray<int>(maxDistance * 2));
                // "k" is an arbitrary variable defined as: k = x - y
                // We're always comparing 2 elements. x is the location of the first element in collection1. y is the location of the second element in collection2
                for (var k = -1 * distance; k <= distance; k += 2)
                {
                    int x;
                    if (distance == 0)
                        x = 0;
                    else if (k == -distance || (k != distance && trace[distance - 1][k - 1] < trace[distance - 1][k + 1]))
                        x = trace[distance - 1][k + 1];
                    else
                        x = trace[distance - 1][k - 1] + 1;

                    var y = x - k;

                    // This "while" is the greedy part of the algorithm - as long as the next elements are the same match them
                    while (x < collection1Wrapper.Length && y < collection2Wrapper.Length && collection1Wrapper[x].Equals(collection2Wrapper[y]))
                    {
                        x = x + 1;
                        y = y + 1;
                    }

                    trace[distance][k] = x;

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
