using IEnumerableCorrelater.Interfaces;

namespace IEnumerableCorrelater.Calculators
{
    public class BasicInsertionCalculator<T> : IInsertionCalculator<T>
    {
        private readonly int value;

        public BasicInsertionCalculator(int value)
        {
            this.value = value;
        }

        public int InsertionCost(T element) => value;
    }
}
