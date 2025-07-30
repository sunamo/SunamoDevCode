namespace SunamoDevCode._sunamo.SunamoTwoWayDictionary;


internal class TwoWayDictionary<T, U>
{
    internal Dictionary<T, U> _d1 = null;
    internal Dictionary<U, T> _d2 = null;
    internal TwoWayDictionary(int c)
    {
        _d1 = new Dictionary<T, U>(c);
        _d2 = new Dictionary<U, T>(c);
    }
    internal TwoWayDictionary()
    {
        _d1 = new Dictionary<T, U>();
        _d2 = new Dictionary<U, T>();
    }
    internal void Add(T key, U value)
    {
        _d1.Add(key, value);
        _d2.Add(value, key);
    }
}