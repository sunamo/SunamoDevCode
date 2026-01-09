namespace SunamoDevCode._sunamo.SunamoCollections;

internal partial class CA
{
    /// <summary>
    /// Removes items from list that contain any pattern from the specified list
    /// </summary>
    /// <param name="files">List to filter</param>
    /// <param name="list">List of patterns to match</param>
    /// <param name="isWildcard">Whether to use wildcard matching</param>
    /// <param name="wildcardIsMatch">Function for wildcard matching (required if isWildcard is true)</param>
    internal static void RemoveWhichContainsList(List<string> files, List<string> list, bool isWildcard, Func<string, string, bool>? wildcardIsMatch = null)
    {
        foreach (var item in list)
        {
            RemoveWhichContains(files, item, isWildcard, wildcardIsMatch);
        }
    }

    /// <summary>
    /// Trims specified characters from the end of all strings in list
    /// </summary>
    /// <param name="list">List to trim</param>
    /// <param name="toTrim">Characters to trim from end</param>
    /// <returns>List with trimmed strings</returns>
    internal static List<string> TrimEnd(List<string> list, params char[] toTrim)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = list[i].TrimEnd(toTrim);
        }

        return list;
    }

    /// <summary>
    /// Joins multiple IList collections into single List
    /// </summary>
    /// <typeparam name="T">Type of elements</typeparam>
    /// <param name="lists">Lists to join</param>
    /// <returns>Combined list with all elements</returns>
    internal static List<T> JoinIList<T>(params IList<T>[] lists)
    {
        List<T> result = new List<T>();
        foreach (var list in lists)
        {
            foreach (var element in list)
            {
                result.Add((T)element);
            }
        }

        return result;
    }

    /// <summary>
    /// Removes empty lines from the beginning of list until first non-empty line
    /// </summary>
    /// <param name="lines">List to process</param>
    internal static void RemoveEmptyLinesToFirstNonEmpty(List<string> lines)
    {
        for (int i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            if (line.Trim() == string.Empty)
            {
                lines.RemoveAt(i);
                i--;
            }
            else
            {
                break;
            }
        }
    }

    /// <summary>
    /// Removes lines at specified indexes
    /// </summary>
    /// <param name="lines">List to modify</param>
    /// <param name="lineIndexesToRemove">Indexes of lines to remove</param>
    internal static void RemoveLines(List<string> lines, List<int> lineIndexesToRemove)
    {
        lineIndexesToRemove.Sort();
        for (int i = lineIndexesToRemove.Count - 1; i >= 0; i--)
        {
            var lineIndex = lineIndexesToRemove[i];
            lines.RemoveAt(lineIndex);
        }
    }

    /// <summary>
    /// Removes strings that are empty after trimming
    /// </summary>
    /// <param name="list">List to filter</param>
    /// <returns>List with empty strings removed</returns>
    internal static List<string> RemoveStringsEmpty2(List<string> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i].Trim() == string.Empty)
            {
                list.RemoveAt(i);
            }
        }

        return list;
    }

    /// <summary>
    /// Wraps all strings in list with specified text on both sides
    /// </summary>
    /// <param name="list">List to modify</param>
    /// <param name="wrapText">Text to add before and after each item</param>
    /// <returns>Modified list</returns>
    internal static List<string> WrapWith(List<string> list, string wrapText)
    {
        return WrapWith(list, wrapText, wrapText);
    }

    /// <summary>
    /// Wraps all strings in list with specified prefix and suffix
    /// Directly edits the input list
    /// </summary>
    /// <param name="list">List to modify</param>
    /// <param name="prefixText">Text to add before each item</param>
    /// <param name="suffixText">Text to add after each item</param>
    /// <returns>Modified list</returns>
    internal static List<string> WrapWith(List<string> list, string prefixText, string suffixText)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = prefixText + list[i] + suffixText;
        }

        return list;
    }

    /// <summary>
    /// Ensures all paths end with backslash
    /// </summary>
    /// <param name="paths">List of paths to modify</param>
    /// <returns>List with backslash added to paths that don't end with it</returns>
    internal static List<string> EnsureBackslash(List<string> paths)
    {
        for (int i = 0; i < paths.Count; i++)
        {
            string path = paths[i];
            if (path[path.Length - 1] != '\\')
            {
                paths[i] = path + "\\";
            }
        }

        return paths;
    }

    /// <summary>
    /// Checks if list contains specified element
    /// </summary>
    /// <typeparam name="T">Type of elements</typeparam>
    /// <param name="list">List to search in</param>
    /// <param name="element">Element to search for</param>
    /// <returns>True if element is found, false otherwise</returns>
    internal static bool ContainsElement<T>(IList<T> list, T element)
    {
        if (list.Count == 0)
        {
            return false;
        }

        foreach (T item in list)
        {
            if (Comparer<T>.Equals(item, element))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Removes items that match the wildcard mask
    /// </summary>
    /// <param name="list">List to filter</param>
    /// <param name="mask">Wildcard mask to match</param>
    internal static void RemoveWildcard(List<string> list, string mask)
    {
        //https://stackoverflow.com/a/15275806
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (SH.MatchWildcard(list[i], mask))
            {
                list.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Checks if text ends with any of the specified suffixes
    /// </summary>
    /// <param name="key">Text to check</param>
    /// <param name="suffixes">Suffixes to check for</param>
    /// <returns>True if text ends with any suffix, false otherwise</returns>
    internal static bool HasPostfix(string key, params string[] suffixes)
    {
        foreach (var item in suffixes)
        {
            if (key.EndsWith(item))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Prepends prefix to all items in list that don't already start with it
    /// </summary>
    /// <param name="prefix">Prefix to add</param>
    /// <param name="list">List to modify</param>
    /// <returns>Modified list</returns>
    internal static List<string> Prepend(string prefix, List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].StartsWith(prefix))
            {
                list[i] = prefix + list[i];
            }
        }

        return list;
    }

    /// <summary>
    /// Converts string array to List of strings
    /// </summary>
    /// <param name="values">Array to convert</param>
    /// <returns>List containing all array elements</returns>
    internal static List<string> ToListString(params string[] values)
    {
        return values.ToList();
    }
}
