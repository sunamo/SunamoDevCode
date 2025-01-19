namespace SunamoDevCode._public.SunamoData.Data;


public class TWithNameTDC<T>
{



    public string name = string.Empty;
    public T t;
    public TWithNameTDC()
    {
    }
    public TWithNameTDC(string name, T t)
    {
        this.name = name;
        this.t = t;
    }
    public override string ToString()
    {
        return name;
    }
    public static TWithNameTDC<T> Get(string nameCb)
    {
        return new TWithNameTDC<T> { name = nameCb };
    }
}