namespace SunamoDevCode._public;

public class OutRef3DC<T, U, V> : OutRefDC<T, U>
{
    public OutRef3DC(T firstValue, U secondValue, V thirdValue) : base(firstValue, secondValue)
    {
        Item3 = thirdValue;
    }
    public V Item3 { get; set; }
}
