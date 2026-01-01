namespace SunamoDevCode;

/// <summary>
/// EN: Helper for handling backslash encoding in strings
/// CZ: Pomocník pro práci s backslash encoding ve stringech
/// </summary>
public class BackslashEncoding
{
    /// <summary>
    /// EN: Removes positions that are inside strings
    /// CZ: Odstraní pozice které jsou uvnitř stringů
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