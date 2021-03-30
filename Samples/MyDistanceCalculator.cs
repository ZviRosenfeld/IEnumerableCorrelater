using System;
using IEnumerableCorrelater.Interfaces;

namespace Samples
{
    class MyDistanceCalculator<T> : IDistanceCalculator<T>
    {
        public uint Distance(T element1, T element2)
        {
            throw new NotImplementedException();
        }
    }
}
