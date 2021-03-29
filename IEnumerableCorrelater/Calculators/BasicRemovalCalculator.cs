using IEnumerableCorrelater.Interfaces;
using System;

namespace IEnumerableCorrelater.Calculators
{
    public class BasicRemovalCalculator<T> : IRemovalCalculator<T>
    {
        private readonly int value;

        public BasicRemovalCalculator(int value)
        {
            if (value <= 0)
                throw new ArgumentException("Removal cost must be greater than 0",nameof(value));

            this.value = value;
        }

        public int RemovalCost(T element) => value;
    }
}
