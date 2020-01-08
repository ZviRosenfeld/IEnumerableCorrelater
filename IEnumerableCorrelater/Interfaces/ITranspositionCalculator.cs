namespace IEnumerableCorrelater.Interfaces
{
    public interface ITranspositionCalculator<T>
    {
        int TranspositionCost(T element1, T element2);
    }
}
