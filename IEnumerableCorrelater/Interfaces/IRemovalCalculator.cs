namespace IEnumerableCorrelater.Interfaces
{
    public interface IRemovalCalculator<T>
    {
        int RemovalCost(T element);
    }
}
