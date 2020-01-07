namespace IEnumerableCorrelater.Interfaces
{
    public interface IStringCorrelater
    {
        CorrelaterResult<char> Compare(string string1, string string2);
    }
}
