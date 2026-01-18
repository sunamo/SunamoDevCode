namespace SunamoDevCode.Data;

/// <summary>
/// Generic class that separates items into two lists: existing and non-existing.
/// </summary>
/// <typeparam name="T">Type of items in the lists.</typeparam>
public class ExistsNonExistsList<T>
{
    /// <summary>
    /// Gets or sets the list of items that exist.
    /// </summary>
    public List<T> Exists { get; set; } = new List<T>();

    /// <summary>
    /// Gets or sets the list of items that do not exist.
    /// </summary>
    public List<T> NonExists { get; set; } = new List<T>();
}