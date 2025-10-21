// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
// Instance variables refactored according to C# conventions
namespace SunamoDevCode;

public class BackslashEncoding
{
    public static void RemoveWhichIsInString(string inputString, List<int> positionsList)
    {
        var fromToDetector = CSharpHelperSunamo.DetectFromToString(inputString);
        for (var index = 0; index < positionsList.Count; index++)
            if (fromToDetector.IsInRange(positionsList[index]))
                positionsList.RemoveAt(index);
    }
}