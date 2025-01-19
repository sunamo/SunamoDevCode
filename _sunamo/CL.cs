using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunamoDevCode._sunamo;

internal class CL
{
    public static string AskForEnter(string whatOrTextWithoutEndingDot, bool appendAfterEnter,
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

    public static string UserMustTypeMultiLine(string v, params string[] anotherPossibleAftermOne)
    {
        string line = null;
        Information(AskForEnter(v, true, ""));
        StringBuilder sb = new();
        //string lastAdd = null;
        while ((line = Console.ReadLine()) != null)
        {
            if (line == "-1") break;
            sb.AppendLine(line);
            if (anotherPossibleAftermOne.Contains(line)) break;
            //lastAdd = line;
        }
        //sb.AppendLine(line);
        var s2 = sb.ToString().Trim();
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
