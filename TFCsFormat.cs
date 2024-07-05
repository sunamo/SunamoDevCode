using FileMs = System.IO.File;

namespace SunamoDevCode;

public partial class TFCsFormat
{
    static readonly List<string> classCodeElements = new List<string>() { "class ", "interface ", "enum ", "struct ", "delegate " };

    static List<string> OnlyToFirst(List<string> d)
    {
        List<string> toFirstCodeElement = new List<string>();

        for (int i = 0; i < d.Count; i++)
        {
            var line = d[i];
            if (classCodeElements.Any(d => line.Contains(d)))
            {
                toFirstCodeElement = d.Take(i + 1).ToList();

                for (var j = toFirstCodeElement.Count - 1; j >= 0; j--)
                {
                    d.RemoveAt(0);
                }

                break;
            }
        }

        return toFirstCodeElement;
    }

    public static void WriteAllText(string p, string c)
    {
        WriteAllTextAsync(p, c).GetAwaiter().GetResult();
    }

    public static void WriteAllLines(string p, List<string> l)
    {
        WriteAllLinesAsync(p, l).GetAwaiter().GetResult();
    }

    public static async Task WriteAllTextAsync(string p, string c)
    {
        if (!p.EndsWith(".cs") || p.EndsWith("GlobalUsings.cs"))
        {
            await FileMs.WriteAllTextAsync(p, c);
            return;
        }

        var l = SHGetLines.GetLines(c);
        await WriteAllLinesAsync(p, l);
    }

    public static async Task WriteAllLinesAsync(string p, List<string> l)
    {
        if (!p.EndsWith(".cs") || p.EndsWith("GlobalUsings.cs"))
        {
            await FileMs.WriteAllLinesAsync(p, l);
            return;
        }

        var toFirstCodeElement = OnlyToFirst(l);

        List<string> usings = new List<string>();
        string ns = string.Empty;


        foreach (var item in toFirstCodeElement)
        {
            if (item.StartsWith("using "))
            {
                usings.Add(item);
            }
            else if (item.StartsWith("namespace "))
            {
                ns = item;
            }
        }

        usings.Add("");
        usings.Add(ns);
        usings.Add("");
        usings.AddRange(l);

        await FileMs.WriteAllTextAsync(p, SHJoin.JoinNL(usings));
    }
}
