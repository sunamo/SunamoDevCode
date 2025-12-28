namespace SunamoDevCode._sunamo.SunamoTwoWayDictionary;

internal class TwoWayDictionary<T, U>
{
    internal Dictionary<T, U> FirstDictionary = null;
    internal Dictionary<U, T> SecondDictionary = null;
    internal TwoWayDictionary(int c)
    {
        FirstDictionary = new Dictionary<T, U>(c);
        SecondDictionary = new Dictionary<U, T>(c);
    }
    internal TwoWayDictionary()
    {
        FirstDictionary = new Dictionary<T, U>();
        SecondDictionary = new Dictionary<U, T>();
    }
    internal void Add(T key, U value)
    {
        FirstDictionary.Add(key, value);
        SecondDictionary.Add(value, key);
    }
}