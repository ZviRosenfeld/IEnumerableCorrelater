using IEnumerableCorrelater.Interfaces;
using System;

namespace IEnumerableCorrelater.Calculators
{
    public class BasicTranspositionCalculator<T> : ITranspositionCalculator<T>
    {
        private readonly int value;

        public BasicTranspositionCalculator(int value)
        {
            if (value < 0)
                throw new ArgumentException("Transposition cost must be at least 0", nameof(value));

            this.value = value;
        }

        public int TranspositionCost(T element1, T element2) => value;
    }
}
