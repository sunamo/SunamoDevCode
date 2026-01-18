namespace SunamoDevCode;

/// <summary>
/// Helper for handling backslash encoding in strings
/// </summary>
public class BackslashEncoding
{
    /// <summary>
    /// Removes positions that are inside strings
    /// </summary>
    /// <param name="inputString">Input string to analyze</param>
    /// <param name="positionsList">List of positions to filter</param>
    public static void RemoveWhichIsInString(string inputString, List<int> positionsList)
    {
        var fromToDetector = CSharpHelperSunamo.DetectFromToString(inputString);
        for (var index = 0; index < positionsList.Count; index++)
            if (fromToDetector.IsInRange(positionsList[index]))
                positionsList.RemoveAt(index);
    }
}