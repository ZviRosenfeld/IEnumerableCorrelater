using IEnumerableCorrelater.Interfaces;
using System;

namespace IEnumerableCorrelater.Calculators
{
    public class BasicDistanceCalculator<T> : IDistanceCalculator<T>
    {
        private readonly int value;

        public BasicDistanceCalculator(int value)
        {
            if (value < 0)
                throw new ArgumentException("Distance cost must be at least 0", nameof(value));

            this.value = value;
        }

        public int Distance(T element1, T element2) => 
            element1.Equals(element2) ? 0 : value;
    }
}
