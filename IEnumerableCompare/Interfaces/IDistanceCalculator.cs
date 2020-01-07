namespace IEnumerableCompare.Interfaces
{
    public interface IDistanceCalculator<T>
    {
        int Distance(T element1, T element2);
    }
}
