namespace IEnumerableCompare
{
    public class CompareResult<T>
    {
        public CompareResult(int distance, T[] bestMatch1, T[] bestMatch2)
        {
            Distance = distance;
            BestMatch1 = bestMatch1;
            BestMatch2 = bestMatch2;
        }

        public int Distance { get; }

        public T[] BestMatch1 { get; }

        public T[] BestMatch2 { get; }
    }
}
