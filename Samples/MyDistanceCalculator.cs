using System;
using IEnumerableCompare.Interfaces;

namespace Samples
{
    class MyDistanceCalculator : IDistanceCalculator<string>
    {
        public int Distance(string element1, string element2)
        {
            throw new NotImplementedException();
        }
    }
}
