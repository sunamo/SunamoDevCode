namespace SunamoDevCode;


public class TWithNameT<T>
{
    /// <summary>
    ///     Just first 5. letters
    /// </summary>
    public string name = string.Empty;
    public T t;
    public TWithNameT()
    {
    }
    public TWithNameT(string name, T t)
    {
        this.name = name;
        this.t = t;
    }
    public override string ToString()
    {
        return name;
    }
    public static TWithNameT<T> Get(string nameCb)
    {
        return new TWithNameT<T> { name = nameCb };
    }
}