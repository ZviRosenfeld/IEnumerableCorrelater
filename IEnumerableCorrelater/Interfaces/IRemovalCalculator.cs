namespace IEnumerableCorrelater.Interfaces
{
    public interface IRemovalCalculator<T>
    {
        uint RemovalCost(T element);
    }
}
