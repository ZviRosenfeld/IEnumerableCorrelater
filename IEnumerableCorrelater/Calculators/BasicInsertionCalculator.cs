using IEnumerableCorrelater.Interfaces;

namespace IEnumerableCorrelater.Calculators
{
    public class BasicInsertionCalculator<T> : IInsertionCalculator<T>
    {
        private readonly uint value;

        public BasicInsertionCalculator(uint value)
        {
            this.value = value;
        }

        public uint InsertionCost(T element) => value;
    }
}
