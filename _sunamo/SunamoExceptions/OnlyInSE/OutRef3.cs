namespace SunamoDevCode;


public class OutRef3<T, U, V> : OutRefDC<T, U>
{
    internal OutRef3(T t, U u, V v) : base(t, u)
    {
        Item3 = v;
    }
    internal V Item3 { get; set; }
}