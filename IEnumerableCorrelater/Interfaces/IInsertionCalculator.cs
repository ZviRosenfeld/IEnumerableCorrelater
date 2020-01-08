namespace IEnumerableCorrelater.Interfaces
{
    public interface IInsertionCalculator<T>
    {
        int InsertionCost(T element);
    }
}
