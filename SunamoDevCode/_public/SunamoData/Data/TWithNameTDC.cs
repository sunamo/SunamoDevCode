namespace SunamoDevCode._public.SunamoData.Data;

/// <summary>
/// Generic container pairing a name string with a value of type T.
/// </summary>
/// <typeparam name="T">Type of the stored value.</typeparam>
public class TWithNameTDC<T>
{
    /// <summary>
    /// Name associated with the value.
    /// </summary>
    public string name = string.Empty;
    /// <summary>
    /// The stored value of type T.
    /// </summary>
    public T t = default!;
    /// <summary>
    /// Default constructor.
    /// </summary>
    public TWithNameTDC()
    {
    }
    /// <summary>
    /// Creates a new instance with the given name and value.
    /// </summary>
    /// <param name="name">Name to associate.</param>
    /// <param name="t">Value to store.</param>
    public TWithNameTDC(string name, T t)
    {
        this.name = name;
        this.t = t;
    }
    /// <summary>
    /// Returns the name string.
    /// </summary>
    /// <returns>The name.</returns>
    public override string ToString()
    {
        return name;
    }
    /// <summary>
    /// Creates a new instance with only the name set.
    /// </summary>
    /// <param name="name">Name to set.</param>
    /// <returns>New instance with the given name.</returns>
    public static TWithNameTDC<T> Get(string name)
    {
        return new TWithNameTDC<T> { name = name };
    }
}