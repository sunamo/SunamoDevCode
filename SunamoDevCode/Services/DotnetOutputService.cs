// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy

namespace SunamoDevCode.Services;

public class DotnetOutputService
{
    public DotnetBuildOutputLine? GetPartsFromDotnetBuildLine(string line2)
    {
        if (line2.StartsWith("MSBuild version"))
        {
            // MSBuild version 17.8.3+195e7f5a3 for .NET
            return null;
        }
        if (line2.Trim() == string.Empty)
        {
            return null;
        }
        var parameter = SHSplit.SplitToParts(line2, 4, ":");
        var p1Trim = parameter[1].Trim();
        string path = null;
        int line = -1;
        int column = -1;
        if (p1Trim.Contains("("))
        {
            var p3 = SHSplit.Split(p1Trim, "(");
            var last = p3[p3.Count - 1];
            p3.RemoveAt(p3.Count - 1);
            var p4 = SHSplit.Split(last, ",");
            line = int.Parse(p4[0]);
            column = int.Parse(p4[1].TrimEnd(')'));
            path = parameter[0] + ":" + string.Join("(", p3);
        }
        else
        {
            path = parameter[0] + ":" + p1Trim;
        }
        var p2 = SHSplit.Split(parameter[2].Trim(), " ");
        string type = null;
        string errorCode = null;
        if (p2.Count > 1)
        {
            type = p2[0];
            errorCode = p2[1];
        }
        else
        {
            return null;
        }
        var message = parameter[3].Trim();
        return new DotnetBuildOutputLine { Path = path, Line = line, Column = column, Type = type, ErrorCode = errorCode, Message = message };
    }
}