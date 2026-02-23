namespace SunamoDevCode._public;

/// <summary>
/// EN: Container for two values (similar to Tuple but mutable)
/// CZ: Kontejner pro dvě hodnoty (podobné Tuple ale měnitelné)
/// </summary>
/// <typeparam name="T">Type of first value</typeparam>
/// <typeparam name="U">Type of second value</typeparam>
public class OutRefDC<T, U>
{
    /// <summary>
    /// Initializes a new instance with two values.
    /// </summary>
    /// <param name="firstValue">First value.</param>
    /// <param name="secondValue">Second value.</param>
    public OutRefDC(T firstValue, U secondValue)
    {
        Item1 = firstValue;
        Item2 = secondValue;
    }
    /// <summary>
    /// Gets or sets the first value.
    /// </summary>
    public T Item1 { get; set; }
    /// <summary>
    /// Gets or sets the second value.
    /// </summary>
    public U Item2 { get; set; }
}