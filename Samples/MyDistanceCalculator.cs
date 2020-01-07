using System;
using IEnumerableCorrelater.Interfaces;

namespace Samples
{
    class MyDistanceCalculator<T> : IDistanceCalculator<T>
    {
        public int Distance(T element1, T element2)
        {
            throw new NotImplementedException();
        }
    }
}
