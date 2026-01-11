namespace SunamoDevCode._sunamo.SunamoStringTrim;

/// <summary>
/// Helper class for string trimming operations.
/// </summary>
internal class SHTrim
{
    /// <summary>
    /// Trims a specific value from both start and end of the text.
    /// </summary>
    /// <param name="text">The text to trim.</param>
    /// <param name="valueToTrim">The value to remove from both ends.</param>
    /// <returns>The trimmed text.</returns>
    internal static string Trim(string text, string valueToTrim)
    {
        text = TrimStart(text, valueToTrim);
        text = TrimEnd(text, valueToTrim);

        return text;
    }

    // EN: Method TrimLeadingNumbersAtStart was removed - inlined in ConstsManager.cs:110

    /// <summary>
    /// Trims a specific suffix from the end of the text repeatedly until it doesn't end with it.
    /// </summary>
    /// <param name="text">The text to trim.</param>
    /// <param name="suffix">The suffix to remove from the end.</param>
    /// <returns>The trimmed text.</returns>
    internal static string TrimEnd(string text, string suffix)
    {
        while (text.EndsWith(suffix)) return text.Substring(0, text.Length - suffix.Length);

        return text;
    }

    // EN: Method TrimStartAndEnd was removed - inlined in XmlLocalisationInterchangeFileFormat2.cs:691

    /// <summary>
    /// Trims a specific prefix from the start of the text repeatedly until it doesn't start with it.
    /// </summary>
    /// <param name="text">The text to trim.</param>
    /// <param name="prefix">The prefix to remove from the start.</param>
    /// <returns>The trimmed text.</returns>
    internal static string TrimStart(string text, string prefix)
    {
        while (text.StartsWith(prefix))
        {
            text = text.Substring(prefix.Length);
        }

        return text;
    }

}