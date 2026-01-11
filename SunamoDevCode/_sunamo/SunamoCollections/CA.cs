// variables names: ok
namespace SunamoDevCode._sunamo.SunamoCollections;

internal partial class CA
{
    internal enum SearchStrategyCA
    {
        FixedSpace,
        AnySpaces,
        ExactlyName
    }

    /// <summary>
    /// Returns indexes of items in list that contain the specified term
    /// </summary>
    /// <param name="list">List to search in</param>
    /// <param name="term">Term to search for</param>
    /// <returns>List of indexes where term was found</returns>
    internal static List<int> ReturnWhichContainsIndexes(IList<string> list, string term)
    {
        var result = new List<int>();
        var i = 0;
        if (list != null)
            foreach (var item in list)
            {
                if (item.Contains(term))
                    result.Add(i);
                i++;
            }

        return result;
    }

    /// <summary>
    /// Returns indexes of terms that are contained in the specified text
    /// </summary>
    /// <param name="text">Text to search in</param>
    /// <param name="terms">Terms to search for</param>
    /// <returns>List of indexes where terms were found</returns>
    internal static List<int> ReturnWhichContainsIndexes(string text, IList<string> terms)
    {
        var result = new List<int>();
        var i = 0;
        foreach (var term in terms)
        {
            if (text.Contains(term))
                result.Add(i);
            i++;
        }

        return result;
    }

    /// <summary>
    /// Returns distinct indexes of parts that contain any of the must-contain terms
    /// </summary>
    /// <param name="parts">Parts to search in</param>
    /// <param name="mustContains">Terms that must be contained</param>
    /// <returns>Distinct list of indexes</returns>
    internal static IList<int> ReturnWhichContainsIndexes(IList<string> parts, IList<string> mustContains)
    {
        var result = new List<int>();
        foreach (var term in mustContains)
            result.AddRange(ReturnWhichContainsIndexes(parts, term));
        result = result.Distinct().ToList();
        return result;
    }

    /// <summary>
    /// Removes items that don't start with the specified prefix
    /// </summary>
    /// <param name="prefix">Prefix to filter by</param>
    /// <param name="list">List to filter</param>
    /// <returns>Filtered list containing only items starting with prefix</returns>
    internal static List<string> StartingWith(string prefix, List<string> list)
    {
        for (var i = list.Count - 1; i >= 0; i--)
            if (!list[i].StartsWith(prefix))
                list.RemoveAt(i);
        return list;
    }

    /// <summary>
    /// Adds prefix to all items in the list
    /// </summary>
    /// <param name="prefix">Prefix to add</param>
    /// <param name="list">List to modify</param>
    /// <returns>Modified list with prefix added to all items</returns>
    internal static List<string> PostfixIfNotEnding(string prefix, List<string> list)
    {
        for (var i = 0; i < list.Count; i++)
            list[i] = prefix + list[i];
        return list;
    }

    /// <summary>
    /// Splits list into groups separated by delimiter
    /// </summary>
    /// <param name="list">List to split</param>
    /// <param name="delimiter">Delimiter to split by</param>
    /// <returns>List of groups</returns>
    internal static List<List<string>> Split(List<string> list, string delimiter)
    {
        var result = new List<List<string>>();
        var currentGroup = new List<string>();
        foreach (var item in list)
            if (item == delimiter)
            {
                result.Add(currentGroup);
                currentGroup.Clear();
            }

        return result;
    }

    /// <summary>
    /// Removes strings that are empty after trimming
    /// </summary>
    /// <param name="list">List to filter</param>
    /// <returns>List with empty strings removed</returns>
    internal static List<string> RemoveStringsEmptyTrimBefore(List<string> list)
    {
        for (var i = list.Count - 1; i >= 0; i--)
            if (list[i].Trim() == string.Empty)
                list.RemoveAt(i);
        return list;
    }

    /// <summary>
    /// Checks if text contains any element from the list
    /// </summary>
    /// <param name="text">Text to search in</param>
    /// <param name="list">List of elements to search for</param>
    /// <returns>True if any element is found, false otherwise</returns>
    internal static bool ContainsAnyFromElementBool(string text, IList<string> list)
    {
        if (list.Count == 1 && list.First() == "*")
            return true;
        foreach (var item in list)
            if (text.Contains(item))
                return true;
        return false;
    }

    /// <summary>
    /// Removes null, empty, and whitespace strings from list
    /// </summary>
    /// <param name="list">List to filter</param>
    internal static void RemoveNullEmptyWs(List<string> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (string.IsNullOrWhiteSpace(list[i]))
            {
                list.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Trims all strings in the list (directly edits input collection)
    /// </summary>
    /// <param name="list">List to trim</param>
    /// <returns>Trimmed list</returns>
    internal static List<string> Trim(List<string> list)
    {
        for (var i = 0; i < list.Count; i++)
            list[i] = list[i].Trim();
        return list;
    }

    /// <summary>
    /// Replaces double or more consecutive newlines with single double newline
    /// </summary>
    /// <param name="text">Text to process</param>
    internal static void DoubleOrMoreMultiLinesToSingle(ref string text)
    {
        text = Regex.Replace(text, @"(\r?\n\s*){2,}", Environment.NewLine + Environment.NewLine);
        text = text.Trim();
    }

    /// <summary>
    /// Trims strings that contain only whitespace
    /// </summary>
    /// <param name="list">List to process</param>
    internal static void TrimWhereIsOnlyWhitespace(List<string> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            var item = list[i];
            if (string.IsNullOrWhiteSpace(item))
            {
                list[i] = list[i].Trim();
            }
        }
    }

    /// <summary>
    /// Replaces substring in text
    /// </summary>
    /// <param name="text">Text to process</param>
    /// <param name="what">Substring to replace</param>
    /// <param name="replacement">Replacement text</param>
    /// <returns>Text with replacements</returns>
    private static string Replace(string text, string what, string replacement)
    {
        return text.Replace(what, replacement);
    }

    /// <summary>
    /// Replaces substring in all strings in the list
    /// </summary>
    /// <param name="list">List to process</param>
    /// <param name="what">Substring to replace</param>
    /// <param name="replacement">Replacement text</param>
    internal static void Replace(List<string> list, string what, string replacement)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = Replace(list[i], what, replacement);
        }
    }

    /// <summary>
    /// Checks if text starts with negation operator (!) and returns tuple with flag and text without operator
    /// </summary>
    /// <param name="text">Text to check</param>
    /// <returns>Tuple with negation flag and text without operator</returns>
    internal static (bool, string) IsNegationTuple(string text)
    {
        if (text[0] == '!')
        {
            text = text.Substring(1);
            return (true, text);
        }

        return (false, text);
    }

    /// <summary>
    /// Removes items starting with specified prefix (supports negation with ! prefix)
    /// </summary>
    /// <param name="prefix">Prefix to match (use ! for negation)</param>
    /// <param name="list">List to filter</param>
    /// <param name="args">Optional arguments for filtering behavior</param>
    internal static void RemoveStartingWith(string prefix, List<string> list, RemoveStartingWithArgs? args = null)
    {
        if (args == null)
        {
            args = new RemoveStartingWithArgs();
        }

        var(isNegated, actualPrefix) = IsNegationTuple(prefix);
        prefix = actualPrefix;
        for (int i = list.Count - 1; i >= 0; i--)
        {
            var value = list[i];
            if (args.TrimBeforeFinding)
            {
                value = value.Trim();
            }

            if (isNegated)
            {
                if (!StartingWith(value, prefix, args.CaseSensitive))
                {
                    list.RemoveAt(i);
                }
            }
            else
            {
                if (StartingWith(value, prefix, args.CaseSensitive))
                {
                    list.RemoveAt(i);
                }
            }
        }
    }

    /// <summary>
    /// Checks if text starts with specified prefix
    /// </summary>
    /// <param name="text">Text to check</param>
    /// <param name="prefix">Prefix to match</param>
    /// <param name="isCaseSensitive">Whether comparison is case sensitive</param>
    /// <returns>True if text starts with prefix</returns>
    internal static bool StartingWith(string text, string prefix, bool isCaseSensitive)
    {
        if (isCaseSensitive)
        {
            return text.StartsWith(prefix);
        }
        else
        {
            return text.ToLower().StartsWith(prefix.ToLower());
        }
    }

    /// <summary>
    /// Finds which prefix from list the text starts with
    /// </summary>
    /// <param name="prefixes">List of prefixes to check</param>
    /// <param name="text">Text to check</param>
    /// <param name="matchedPrefix">Out parameter with matched prefix</param>
    /// <returns>Text if match found, null otherwise</returns>
    internal static string? StartWith(List<string> prefixes, string text, out string? matchedPrefix)
    {
        matchedPrefix = null;
        if (prefixes != null)
        {
            foreach (var prefix in prefixes)
            {
                if (text.StartsWith(prefix))
                {
                    matchedPrefix = prefix;
                    return text;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Removes items that contain the specified pattern
    /// </summary>
    /// <param name="list">List to filter</param>
    /// <param name="pattern">Pattern to match</param>
    /// <param name="isWildcard">Whether to use wildcard matching</param>
    /// <param name="wildcardIsMatch">Function for wildcard matching (required if isWildcard is true)</param>
    internal static void RemoveWhichContains(List<string> list, string pattern, bool isWildcard, Func<string, string, bool>? wildcardIsMatch)
    {
        if (isWildcard)
        {
            if (wildcardIsMatch == null)
            {
                throw new ArgumentNullException(nameof(wildcardIsMatch), "Wildcard match function is required when isWildcard is true");
            }

            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (wildcardIsMatch(list[i], pattern))
                {
                    list.RemoveAt(i);
                }
            }
        }
        else
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].Contains(pattern))
                {
                    list.RemoveAt(i);
                }
            }
        }
    }
}
