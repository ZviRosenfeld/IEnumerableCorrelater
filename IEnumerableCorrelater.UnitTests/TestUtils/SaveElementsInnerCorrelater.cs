using IEnumerableCorrelater.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IEnumerableCorrelater.UnitTests.TestUtils
{
    /// <summary>
    /// This Correlater saves all the collections it is given to correlate. As a result, it's useful for testing correlation wrappers.
    /// </summary>
    class SaveElementsInnerCorrelater<T> : ICorrelater<T>
    {
        public event Action<int, int> OnProgressUpdate;

        public readonly List<Tuple<T[], T[]>> CorrelatedCollections = new List<Tuple<T[], T[]>>();

        public CorrelaterResult<T> Correlate(IEnumerable<T> collection1, IEnumerable<T> collection2, CancellationToken cancellationToken = default)
        {
            CorrelatedCollections.Add(new Tuple<T[], T[]>(collection1.ToArray(), collection2.ToArray()));
            return new CorrelaterResult<T>(0, collection1.ToArray(), collection2.ToArray());
        }
    }
}
