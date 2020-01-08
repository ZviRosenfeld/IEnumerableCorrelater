using IEnumerableCorrelater.Interfaces;

namespace IEnumerableCorrelater.Calculators
{
    public class BasicDistanceCalculator<T> : IDistanceCalculator<T>
    {
        private readonly int value;

        public BasicDistanceCalculator(int value)
        {
            this.value = value;
        }

        public int Distance(T element1, T element2) => 
            element1.Equals(element2) ? 0 : value;
    }
}
