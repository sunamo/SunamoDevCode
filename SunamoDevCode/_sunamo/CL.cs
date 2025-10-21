// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy

namespace SunamoDevCode._sunamo;

internal class CL
{
    internal static string AskForEnter(string whatOrTextWithoutEndingDot, bool appendAfterEnter,
        string returnWhenIsNotNull)
    {
        if (returnWhenIsNotNull == null)
        {
            if (appendAfterEnter)
                whatOrTextWithoutEndingDot = FromKey("Enter") + " " + whatOrTextWithoutEndingDot + " ";
            whatOrTextWithoutEndingDot +=
                ". " + FromKey("ForExitEnter") + " -1. Is possible enter also nothing - just enter";
            return whatOrTextWithoutEndingDot;
        }
        return returnWhenIsNotNull;
    }
    private static string FromKey(string v)
    {
        return v;
    }
    internal static string UserMustTypeMultiLine(string v, params string[] anotherPossibleAftermOne)
    {
        string line = null;
        Information(AskForEnter(v, true, ""));
        StringBuilder stringBuilder = new();
        //string lastAdd = null;
        while ((line = Console.ReadLine()) != null)
        {
            if (line == "-1") break;
            stringBuilder.AppendLine(line);
            if (anotherPossibleAftermOne.Contains(line)) break;
            //lastAdd = line;
        }
        //stringBuilder.AppendLine(line);
        var s2 = stringBuilder.ToString().Trim();
        return s2;
    }
    private static void Information(string value)
    {
        Console.WriteLine(value);
    }
    internal static void WriteLine(string message)
    {
        Console.WriteLine(message);
    }
}