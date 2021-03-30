namespace IEnumerableCorrelater.Interfaces
{
    public interface IInsertionCalculator<T>
    {
        uint InsertionCost(T element);
    }
}
