namespace SunamoDevCode._sunamo.SunamoStringGetLines;

/// <summary>
/// Helper class for getting lines from text strings.
/// Handles various newline formats (Windows CRLF, Unix LF, Mac CR).
/// </summary>
internal class SHGetLines
{
    /// <summary>
    /// Splits text into lines by all possible newline combinations.
    /// Handles: \r\n, \n\r, \r, \n
    /// </summary>
    /// <param name="text">The text to split into lines.</param>
    /// <returns>List of lines.</returns>
    internal static List<string> GetLines(string text)
    {
        var lines = text.Split(new string[] { "\r\n", "\n\r" }, StringSplitOptions.None).ToList();
        SplitByUnixNewline(lines);
        return lines;
    }

    /// <summary>
    /// Splits lines by Unix-style newline characters (\r and \n separately).
    /// </summary>
    /// <param name="lines">The list of lines to further split.</param>
    private static void SplitByUnixNewline(List<string> lines)
    {
        SplitBy(lines, "\r");
        SplitBy(lines, "\n");
    }

    /// <summary>
    /// Splits lines by a specific delimiter character.
    /// </summary>
    /// <param name="lines">The list of lines to split.</param>
    /// <param name="delimiter">The delimiter character to split by.</param>
    private static void SplitBy(List<string> lines, string delimiter)
    {
        for (int i = lines.Count - 1; i >= 0; i--)
        {
            if (delimiter == "\r")
            {
                var windowsNewlineParts = lines[i].Split(new string[] { "\r\n" }, StringSplitOptions.None);
                var reverseNewlineParts = lines[i].Split(new string[] { "\n\r" }, StringSplitOptions.None);

                if (windowsNewlineParts.Length > 1)
                {
                    ThrowEx.Custom("cannot contain any \r\n, pass already split by this pattern");
                }
                else if (reverseNewlineParts.Length > 1)
                {
                    ThrowEx.Custom("cannot contain any \n\r, pass already split by this pattern");
                }
            }

            var splitParts = lines[i].Split(new string[] { delimiter }, StringSplitOptions.None);

            if (splitParts.Length > 1)
            {
                InsertOnIndex(lines, splitParts.ToList(), i);
            }
        }
    }

    /// <summary>
    /// Inserts split lines at a specific index in the list, replacing the original line.
    /// </summary>
    /// <param name="lines">The list of lines to modify.</param>
    /// <param name="splitLines">The split lines to insert.</param>
    /// <param name="index">The index where to insert the split lines.</param>
    private static void InsertOnIndex(List<string> lines, List<string> splitLines, int index)
    {
        splitLines.Reverse();

        lines.RemoveAt(index);

        foreach (var item in splitLines)
        {
            lines.Insert(index, item);
        }
    }
}