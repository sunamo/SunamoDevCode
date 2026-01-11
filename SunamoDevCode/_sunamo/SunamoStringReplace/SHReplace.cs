namespace SunamoDevCode._sunamo.SunamoStringReplace;

/// <summary>
/// Helper class for string replacement operations.
/// </summary>
internal class SHReplace
{
    /// <summary>
    /// Replaces the first occurrence of a value in text and returns the index where it was found.
    /// </summary>
    /// <param name="text">The text to search and modify.</param>
    /// <param name="searchValue">The value to search for.</param>
    /// <param name="replacement">The replacement text.</param>
    /// <param name="foundIndex">Output: Index where the value was found (-1 if not found or already set).</param>
    /// <returns>The modified text.</returns>
    internal static string ReplaceWithIndex(string text, string searchValue, string replacement, ref int foundIndex)
    {
        if (foundIndex == -1)
        {
            foundIndex = text.IndexOf(searchValue);
            if (foundIndex != -1)
            {
                text = text.Remove(foundIndex, searchValue.Length);
                text = text.Insert(foundIndex, replacement);
            }
        }

        return text;
    }

    /// <summary>
    /// Replaces all occurrences of multiple search values with a single replacement value.
    /// </summary>
    /// <param name="text">The text to modify.</param>
    /// <param name="replacement">The replacement text.</param>
    /// <param name="searchValues">The values to search for and replace.</param>
    /// <returns>The modified text.</returns>
    internal static string ReplaceAll(string text, string replacement, params string[] searchValues)
    {
        foreach (var item in searchValues)
        {
            if (string.IsNullOrEmpty(item))
            {
                return text;
            }
        }

        foreach (var item in searchValues)
        {
            text = text.Replace(item, replacement);
        }
        return text;
    }

    /// <summary>
    /// Replaces only the first occurrence of a pattern using regex.
    /// </summary>
    /// <param name="input">The input text.</param>
    /// <param name="pattern">The regex pattern to search for.</param>
    /// <param name="replacement">The replacement text.</param>
    /// <returns>The modified text.</returns>
    internal static string ReplaceOnce(string input, string pattern, string replacement)
    {
        return new Regex(pattern).Replace(input, replacement, 1);
    }
}