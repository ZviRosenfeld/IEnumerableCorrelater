using IEnumerableCorrelater.Interfaces;
using System;

namespace IEnumerableCorrelater.Calculators
{
    public class BasicInsertionCalculator<T> : IInsertionCalculator<T>
    {
        private readonly int value;

        public BasicInsertionCalculator(int value)
        {
            if (value <= 0)
                throw new ArgumentException("Insertion cost must be greater than 0", nameof(value));

            this.value = value;
        }

        public int InsertionCost(T element) => value;
    }
}
