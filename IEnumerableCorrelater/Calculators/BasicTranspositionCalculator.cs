using IEnumerableCorrelater.Interfaces;

namespace IEnumerableCorrelater.Calculators
{
    public class BasicTranspositionCalculator<T> : ITranspositionCalculator<T>
    {
        private readonly int value;

        public BasicTranspositionCalculator(int value)
        {
            this.value = value;
        }

        public int TranspositionCost(T element1, T element2) => value;
    }
}
