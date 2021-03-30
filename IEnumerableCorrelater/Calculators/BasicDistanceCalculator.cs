using IEnumerableCorrelater.Interfaces;

namespace IEnumerableCorrelater.Calculators
{
    public class BasicDistanceCalculator<T> : IDistanceCalculator<T>
    {
        private readonly uint value;

        public BasicDistanceCalculator(uint value)
        {
            this.value = value;
        }

        public uint Distance(T element1, T element2) => 
            element1.Equals(element2) ? 0 : value;
    }
}
