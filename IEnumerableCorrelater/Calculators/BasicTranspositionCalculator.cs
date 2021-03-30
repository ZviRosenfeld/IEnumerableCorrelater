using IEnumerableCorrelater.Interfaces;

namespace IEnumerableCorrelater.Calculators
{
    public class BasicTranspositionCalculator<T> : ITranspositionCalculator<T>
    {
        private readonly uint value;

        public BasicTranspositionCalculator(uint value)
        {
            this.value = value;
        }

        public uint TranspositionCost(T element1, T element2) => value;
    }
}
