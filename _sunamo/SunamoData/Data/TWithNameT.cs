namespace SunamoDevCode;


internal class TWithNameT<T>
{
    /// <summary>
    ///     Just first 5. letters
    /// </summary>
    internal string name = string.Empty;
    internal T t;
    internal TWithNameT()
    {
    }
    internal TWithNameT(string name, T t)
    {
        this.name = name;
        this.t = t;
    }
    public override string ToString()
    {
        return name;
    }
    internal static TWithNameT<T> Get(string nameCb)
    {
        return new TWithNameT<T> { name = nameCb };
    }
}