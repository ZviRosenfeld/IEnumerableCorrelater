namespace IEnumerableCorrelater.Interfaces
{
    public interface ITranspositionCalculator<T>
    {
        uint TranspositionCost(T element1, T element2);
    }
}
