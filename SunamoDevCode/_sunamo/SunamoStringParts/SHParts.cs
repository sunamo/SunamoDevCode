namespace SunamoDevCode._sunamo.SunamoStringParts;

/// <summary>
/// String Helper for working with string parts.
/// </summary>
internal class SHParts
{
    /// <summary>
    /// Removes everything after the first occurrence of the specified string.
    /// </summary>
    /// <param name="text">The input text.</param>
    /// <param name="searchString">The string to search for.</param>
    /// <returns>Text with everything after the first occurrence removed, or original text if not found.</returns>
    internal static string RemoveAfterFirst(string text, string searchString)
    {
        int index = text.IndexOf(searchString);
        if (index == -1 || index == text.Length - 1)
        {
            return text;
        }

        string result = text.Remove(index);
        return result;
    }
    // Metoda KeepAfterFirst byla odstraněna - inlined v TypeScriptHelper.cs:79

    // Metoda RemoveAfterLast byla odstraněna - inlined v SolutionFolderSerialize.cs:57

    /// <summary>
    /// Removes everything after the first occurrence of the specified character.
    /// </summary>
    /// <param name="text">The input text.</param>
    /// <param name="searchChar">The character to search for.</param>
    /// <returns>Text with everything after the first occurrence removed, or original text if not found.</returns>
    internal static string RemoveAfterFirst(string text, char searchChar)
    {
        int index = text.IndexOf(searchChar);
        return index == -1 || index == text.Length - 1 ? text : text.Substring(0, index);
    }

}