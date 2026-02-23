namespace SunamoDevCode._sunamo.SunamoTwoWayDictionary;

/// <summary>
/// Bidirectional dictionary allowing lookups in both directions between two types.
/// </summary>
/// <typeparam name="T">Type of the first key.</typeparam>
/// <typeparam name="U">Type of the second key.</typeparam>
internal class TwoWayDictionary<T, U> where T : notnull where U : notnull
{
    /// <summary>
    /// Dictionary mapping from first key type to second key type.
    /// </summary>
    internal Dictionary<T, U> FirstToSecond { get; set; } = null!;
    /// <summary>
    /// Dictionary mapping from second key type to first key type.
    /// </summary>
    internal Dictionary<U, T> SecondToFirst { get; set; } = null!;

    /// <summary>
    /// Creates a new bidirectional dictionary with the specified initial capacity.
    /// </summary>
    /// <param name="capacity">Initial capacity for both internal dictionaries.</param>
    internal TwoWayDictionary(int capacity)
    {
        FirstToSecond = new Dictionary<T, U>(capacity);
        SecondToFirst = new Dictionary<U, T>(capacity);
    }

    /// <summary>
    /// Creates a new bidirectional dictionary with default capacity.
    /// </summary>
    internal TwoWayDictionary()
    {
        FirstToSecond = new Dictionary<T, U>();
        SecondToFirst = new Dictionary<U, T>();
    }

    /// <summary>
    /// Adds a bidirectional mapping between the given key and value.
    /// </summary>
    /// <param name="key">Key for the first-to-second direction.</param>
    /// <param name="value">Value for the first-to-second direction (also key in reverse).</param>
    internal void Add(T key, U value)
    {
        FirstToSecond.Add(key, value);
        SecondToFirst.Add(value, key);
    }
}