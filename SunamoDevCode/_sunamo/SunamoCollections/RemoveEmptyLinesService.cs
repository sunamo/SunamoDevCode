namespace SunamoDevCode._sunamo.SunamoCollections;

/// <summary>
/// Service for removing empty lines from collections
/// </summary>
internal class RemoveEmptyLinesService
{
    /// <summary>
    /// Removes empty lines from both start and end of list
    /// </summary>
    /// <param name="lines">List of lines to process</param>
    internal void RemoveEmptyLinesFromStartAndEnd(List<string> lines)
    {
        RemoveEmptyLinesToFirstNonEmpty(lines);
        RemoveEmptyLinesFromBack(lines);
    }

    /// <summary>
    /// Removes empty lines from the beginning until first non-empty line
    /// </summary>
    /// <param name="lines">List of lines to process</param>
    internal void RemoveEmptyLinesToFirstNonEmpty(List<string> lines)
    {
        for (var i = 0; i < lines.Count; i++)
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
    /// Removes empty lines from the end until first non-empty line
    /// </summary>
    /// <param name="lines">List of lines to process</param>
    internal void RemoveEmptyLinesFromBack(List<string> lines)
    {
        for (var i = lines.Count - 1; i >= 0; i--)
        {
            var line = lines[i];
            if (line.Trim() == string.Empty)
                lines.RemoveAt(i);
            else
                break;
        }
    }
}