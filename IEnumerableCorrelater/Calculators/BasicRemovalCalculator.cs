using IEnumerableCorrelater.Interfaces;

namespace IEnumerableCorrelater.Calculators
{
    public class BasicRemovalCalculator<T> : IRemovalCalculator<T>
    {
        private readonly int value;

        public BasicRemovalCalculator(int value)
        {
            this.value = value;
        }

        public int RemovalCost(T element) => value;
    }
}
