namespace SunamoDevCode;

/// <summary>
/// Parser for C# code, specifically for handling const declarations.
/// </summary>
public class CSharpParser
{
    /// <summary>
    /// Public modifier keyword with trailing space.
    /// </summary>
    public const string PublicModifier = "public ";

    /// <summary>
    /// Static readonly modifier keywords with trailing space.
    /// </summary>
    public const string StaticReadonlyModifier = "static readonly ";



    /// <summary>
    ///     Directly save to file
    ///     In A2 will be what can't be deleted, when will be > 0, ThrowException
    /// </summary>
    /// <param name="file"></param>
    /// <param name="remove"></param>
    public static
#if ASYNC
        async Task
#else
    void
#endif
        RemoveConsts(string file, List<string> remove)
    {
        remove.Insert(0, null);

        // EN: Inlined from CAIndexesWithNull.IndexesWithNull - gets indexes of null values in collection
        // CZ: Inlined from CAIndexesWithNull.IndexesWithNull - získává indexy null hodnot v kolekci
        List<int> nullIndexes = new List<int>();
        int index = 0;
        foreach (var item in remove)
        {
            if (item == null)
            {
                nullIndexes.Add(index);
            }
            index++;
        }

        var lines = SHGetLines.GetLines(
#if ASYNC
            await
#endif
                File.ReadAllTextAsync(file)).ToList();

        for (var i = lines.Count - 1; i >= 0; i--)
        {
            var text = lines[i].Trim();
            if (text.Contains(XmlLocalisationInterchangeFileFormatSunamo.Cs))
            {
                var key = XmlLocalisationInterchangeFileFormatSunamo.GetConstsFromLine(text);
                var keyIndex = remove.IndexOf(key);
                if (keyIndex != -1)
                {
                    lines.RemoveAt(i);
                    remove.RemoveAt(keyIndex);
                }
            }
        }

        await File.WriteAllLinesAsync(file, lines);
        if (remove.Count > 0)
        {
            throw new Exception("Cant be deleted in XlfKeys: " + string.Join(",", remove));
        }
    }

    /// <summary>
    /// Parses const declarations from lines, tracking the first occurrence index.
    /// </summary>
    /// <param name="lines">Lines of C# code to parse.</param>
    /// <param name="first">Output parameter for the index of the first const declaration found.</param>
    /// <returns>List of const keys found in the lines.</returns>
    public static List<string> ParseConsts(List<string> lines, out int first)
    {
        var keys = new List<string>();
        first = -1;
        for (var i = 0; i < lines.Count; i++)
        {
            var text = lines[i].Trim();
            if (text.Contains(XmlLocalisationInterchangeFileFormatSunamo.Cs))
            {
                if (first == -1) first = i;

                var key = XmlLocalisationInterchangeFileFormatSunamo.GetConstsFromLine(text);
                keys.Add(key);
            }
        }

        return keys;
    }

    /// <summary>
    /// Parses const declarations from all lines without filtering.
    /// </summary>
    /// <param name="lines">Lines of C# code to parse.</param>
    /// <returns>List of const keys found in the lines.</returns>
    public static List<string> ParseConstsAllLines(List<string> lines)
    {
        var keys = new List<string>();
        for (var i = 0; i < lines.Count; i++)
        {
            var text = lines[i].Trim();
            var key = XmlLocalisationInterchangeFileFormatSunamo.GetConstsFromLine(text);
            keys.Add(key);
        }

        return keys;
    }
}