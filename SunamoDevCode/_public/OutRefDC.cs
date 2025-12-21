namespace SunamoDevCode._public;

public class OutRefDC<T, U>
{
    public OutRefDC(T t, U u)
    {
        Item1 = t;
        Item2 = u;
    }
    public T Item1 { get; set; }
    public U Item2 { get; set; }
}