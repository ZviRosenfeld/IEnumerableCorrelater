using IEnumerableCorrelater.Interfaces;

namespace IEnumerableCorrelater.Calculators
{
    public class BasicRemovalCalculator<T> : IRemovalCalculator<T>
    {
        private readonly uint value;

        public BasicRemovalCalculator(uint value)
        {
            this.value = value;
        }

        public uint RemovalCost(T element) => value;
    }
}
