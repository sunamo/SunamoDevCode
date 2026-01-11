namespace SunamoDevCode._sunamo.SunamoCollectionsGeneric;

/// <summary>
/// Collection Array Generic helper methods
/// </summary>
internal class CAG
{
    /// <summary>
    /// Return what exists in both lists
    /// Modify both firstList and secondList - keep only which is only in one
    /// </summary>
    /// <param name="firstList">First list to compare</param>
    /// <param name="secondList">Second list to compare</param>
    /// <returns>List of items that exist in both lists</returns>
    internal static List<T> CompareList<T>(List<T> firstList, List<T> secondList) where T : IEquatable<T>
    {
        List<T> existsInBoth = new List<T>();

        int foundIndex = -1;

        for (int i = secondList.Count - 1; i >= 0; i--)
        {
            T currentItem = secondList[i];
            foundIndex = firstList.IndexOf(currentItem);

            if (foundIndex != -1)
            {
                existsInBoth.Add(currentItem);
                secondList.RemoveAt(i);
                firstList.RemoveAt(foundIndex);
            }
        }

        for (int i = firstList.Count - 1; i >= 0; i--)
        {
            T currentItem = firstList[i];
            foundIndex = secondList.IndexOf(currentItem);

            if (foundIndex != -1)
            {
                existsInBoth.Add(currentItem);
                firstList.RemoveAt(i);
                secondList.RemoveAt(foundIndex);
            }
        }

        return existsInBoth;
    }

    /// <summary>
    /// Gets duplicate items from list
    /// </summary>
    /// <typeparam name="T">Type of list elements</typeparam>
    /// <param name="list">List to check for duplicates</param>
    /// <returns>List of duplicate items</returns>
    internal static List<T> GetDuplicities<T>(List<T> list)
    {
        List<T> alreadyProcessed;
        return GetDuplicities<T>(list, out alreadyProcessed);
    }

    /// <summary>
    /// Gets duplicate items from list with already processed items output
    /// </summary>
    /// <typeparam name="T">Type of list elements</typeparam>
    /// <param name="list">List to check for duplicates</param>
    /// <param name="alreadyProcessed">Output list of already processed items</param>
    /// <returns>List of duplicate items</returns>
    internal static List<T> GetDuplicities<T>(List<T> list, out List<T> alreadyProcessed)
    {
        alreadyProcessed = new List<T>(list.Count);
        CollectionWithoutDuplicatesDC<T> duplicated = new CollectionWithoutDuplicatesDC<T>();
        foreach (var currentItem in list)
        {
            if (alreadyProcessed.Contains(currentItem))
            {
                duplicated.Add(currentItem);
            }
            else
            {
                alreadyProcessed.Add(currentItem);
            }
        }
        return duplicated.Collection;
    }

    /// <summary>
    /// Direct edit - Remove duplicities from list
    /// In return value is from every one instance
    /// In foundedDuplicities is every duplicities (maybe the same more times)
    /// </summary>
    /// <typeparam name="T">Type of list elements</typeparam>
    /// <param name="list">List to remove duplicates from</param>
    /// <param name="foundedDuplicities">Output list of found duplicates</param>
    /// <returns>List of unique items</returns>
    internal static List<T> RemoveDuplicitiesList<T>(IList<T> list, out List<T> foundedDuplicities)
    {
        foundedDuplicities = new List<T>();
        List<T> uniqueItems = new List<T>();
        for (int i = list.Count - 1; i >= 0; i--)
        {
            var currentItem = list[i];
            if (!uniqueItems.Contains(currentItem))
            {
                uniqueItems.Add(currentItem);
            }
            else
            {
                list.RemoveAt(i);
                foundedDuplicities.Add(currentItem);
            }
        }

        return uniqueItems;
    }

    /// <summary>
    /// Converts params array to List
    /// </summary>
    /// <typeparam name="T">Type of array elements</typeparam>
    /// <param name="items">Items to convert to list</param>
    /// <returns>List containing all items</returns>
    internal static List<T> ToList<T>(params T[] items)
    {
        return new List<T>(items);
    }
}
