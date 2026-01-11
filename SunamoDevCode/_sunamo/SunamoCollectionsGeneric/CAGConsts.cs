namespace SunamoDevCode._sunamo.SunamoCollectionsGeneric;

/// <summary>
/// Collection Array Generic constants
/// This must be here because SunamoValues cannot inherit from SunamoCollectionGeneric - it would create a cycle dependency
/// </summary>
internal class CAGConsts
{
    /// <summary>
    /// Converts params array to List
    /// </summary>
    /// <typeparam name="T">Type of array elements</typeparam>
    /// <param name="items">Items to convert to list</param>
    /// <returns>List containing all items</returns>
    internal static List<T> ToList<T>(params T[] items)
    {
        return items.ToList();
    }
}
