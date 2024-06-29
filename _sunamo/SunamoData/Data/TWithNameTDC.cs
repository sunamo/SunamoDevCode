namespace SunamoDevCode;


public class TWithNameTDC<T>
{
    /// <summary>
    ///     Just first 5. letters
    /// </summary>
    internal string name = string.Empty;
    internal T t;
    internal TWithNameTDC()
    {
    }
    internal TWithNameTDC(string name, T t)
    {
        this.name = name;
        this.t = t;
    }
    public override string ToString()
    {
        return name;
    }
    internal static TWithNameTDC<T> Get(string nameCb)
    {
        return new TWithNameTDC<T> { name = nameCb };
    }
}