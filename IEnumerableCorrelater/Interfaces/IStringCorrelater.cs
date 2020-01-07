namespace IEnumerableCorrelater.Interfaces
{
    public interface IStringCorrelater
    {
        CorrelaterResult<char> Correlate(string string1, string string2);
    }
}
