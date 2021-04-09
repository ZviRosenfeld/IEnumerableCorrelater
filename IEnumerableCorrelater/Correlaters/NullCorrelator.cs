using IEnumerableCorrelater.Interfaces;
using IEnumerableCorrelater.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IEnumerableCorrelater.Correlaters
{
    /// <summary>
    /// Returns a trivial 'match' that contains the first collection and then the second collection 
    /// </summary>
    public class NullCorrelator<T> : ICorrelater<T>
    {
        public event Action<int, int> OnProgressUpdate;

        public CorrelaterResult<T> Correlate(IEnumerable<T> collection1, IEnumerable<T> collection2, CancellationToken cancellationToken = default)
        {
            return new CorrelaterResult<T>(collection1.Count() + collection2.Count(),
                collection1.Concat(collection2.Count().GetNNullElemenets<T>()).ToArray(),
                collection1.Count().GetNNullElemenets<T>().Concat(collection2).ToArray());
        }
    }
}
