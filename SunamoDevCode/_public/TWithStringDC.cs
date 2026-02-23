namespace SunamoDevCode._public;

/// <summary>
/// Generic container pairing a value of type T with a string path.
/// </summary>
/// <typeparam name="T">Type of the stored value.</typeparam>
public class TWithStringDC<T>
{
    /// <summary>
    /// String path associated with the value.
    /// </summary>
    public string path = null!;
    /// <summary>
    /// The stored value of type T.
    /// </summary>
    public T t = default!;
    /// <summary>
    /// Default constructor.
    /// </summary>
    public TWithStringDC()
    {
    }
    /// <summary>
    /// Creates a new instance with the given value and path.
    /// </summary>
    /// <param name="t">Value to store.</param>
    /// <param name="path">Associated string path.</param>
    public TWithStringDC(T t, string path)
    {
        this.t = t;
        this.path = path;
    }
    /// <summary>
    /// Returns the string path.
    /// </summary>
    /// <returns>The path string.</returns>
    public override string ToString()
    {
        return path;
    }
}