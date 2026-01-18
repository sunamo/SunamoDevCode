namespace SunamoDevCode._public;

/// <summary>
/// EN: Container for three values (similar to Tuple but mutable)
/// CZ: Kontejner pro tři hodnoty (podobné Tuple ale měnitelné)
/// </summary>
/// <typeparam name="T">Type of first value</typeparam>
/// <typeparam name="U">Type of second value</typeparam>
/// <typeparam name="V">Type of third value</typeparam>
public class OutRef3DC<T, U, V> : OutRefDC<T, U>
{
    public OutRef3DC(T firstValue, U secondValue, V thirdValue) : base(firstValue, secondValue)
    {
        Item3 = thirdValue;
    }
    public V Item3 { get; set; }
}