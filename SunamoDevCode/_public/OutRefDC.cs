// variables names: ok
namespace SunamoDevCode._public;

/// <summary>
/// EN: Container for two values (similar to Tuple but mutable)
/// CZ: Kontejner pro dvě hodnoty (podobné Tuple ale měnitelné)
/// </summary>
/// <typeparam name="T">Type of first value</typeparam>
/// <typeparam name="U">Type of second value</typeparam>
public class OutRefDC<T, U>
{
    public OutRefDC(T firstValue, U secondValue)
    {
        Item1 = firstValue;
        Item2 = secondValue;
    }
    public T Item1 { get; set; }
    public U Item2 { get; set; }
}