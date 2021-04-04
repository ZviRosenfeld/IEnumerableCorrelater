namespace IEnumerableCorrelater.Utils
{
    /// <summary>
    /// An array that can be accessed with negative indexes.
    /// A negative array index is interpreted as reading from the end of the array.
    /// </summary>
    public class NegativeArray<T>
    {
        private T[] storage;

        public NegativeArray(int size)
        {
            storage = new T[size];
        }

        public T this[int index]
        {
            get => storage[GetActualIndex(index)];
            set => storage[GetActualIndex(index)] = value;
        }

        public int Length => storage.Length;

        private int GetActualIndex(int index) => index < 0 ? storage.Length + index : index;
    }
}
