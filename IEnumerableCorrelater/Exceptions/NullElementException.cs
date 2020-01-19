namespace IEnumerableCorrelater.Exceptions
{
    public class NullElementException : EnumerableCorrelaterException
    {
        public NullElementException(string collectionName, int index) : 
            base($"The elements at index {index} in collection {collectionName} was null. EnumerableCorrelater doesn't support comparing collections with null elements in them. If you need null elements, consider using the 'Null Object Pattern'.")
        {
        }
    }
}
