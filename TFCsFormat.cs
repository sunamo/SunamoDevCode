namespace SunamoDevCode;

using FileMs = File;

public class TFCsFormat
{
    private static readonly List<string> classCodeElements = new()
        { "class ", "interface ", "enum ", "struct ", "delegate " };

    private static List<string> OnlyToFirst(List<string> d)
    {
        var toFirstCodeElement = new List<string>();

        for (var i = 0; i < d.Count; i++)
        {
            var line = d[i];
            if (classCodeElements.Any(d => line.Contains(d)))
            {
                for (var y = i - 1; y >= 0; y--)
                    if (d[y].StartsWith("//"))
                        i--;
                    else
                        break;

                toFirstCodeElement = d.Take(i).ToList();

                for (var j = toFirstCodeElement.Count - 1; j >= 0; j--) d.RemoveAt(0);

                break;
            }
        }

        return toFirstCodeElement;
    }

    public static void WriteAllTextSync(string p, string c)
    {
        WriteAllText(p, c).GetAwaiter().GetResult();
    }

    public static void WriteAllLinesSync(string p, IEnumerable<string> l)
    {
        WriteAllLines(p, l).GetAwaiter().GetResult();
    }

    public static async Task WriteAllText(string p, string c)
    {
        if (!p.EndsWith(".cs") || p.EndsWith("GlobalUsings.cs"))
        {
            await FileMs.WriteAllTextAsync(p, c);
            return;
        }

        var l = SHGetLines.GetLines(c);
        await WriteAllLines(p, l);
    }

    public static async Task WriteAllLines(string p, IEnumerable<string> l)
    {
        if (!p.EndsWith(".cs") || p.EndsWith("GlobalUsings.cs"))
        {
            await FileMs.WriteAllLinesAsync(p, l);
            return;
        }

        var l2 = l.ToList();

        var toFirstCodeElement = OnlyToFirst(l2);

        var usings = new List<string>();
        var ns = string.Empty;


        foreach (var item in toFirstCodeElement)
            if (item.StartsWith("using "))
                usings.Add(item);
            else if (item.StartsWith("namespace ")) ns = item;

        if (ns == string.Empty)
        {
            // todo doplnit ns
        }

        if (usings.Count != 0) usings.Add("");

        usings.Insert(0, ns);
        usings.Insert(0, "");
        usings.AddRange(l2);

        await FileMs.WriteAllTextAsync(p, SHJoin.JoinNL(usings));
    }
}