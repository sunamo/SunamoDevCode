namespace SunamoDevCode;

public class CSharpParser
{
    public const string p = "public ";


    //public const string c = "const string";

    //public static string c => XmlLocalisationInterchangeFileFormatSunamo.cs;



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

        var ind = CAIndexesWithNull.IndexesWithNull(remove);

        var lines = SHGetLines.GetLines(
#if ASYNC
            await
#endif
                File.ReadAllTextAsync(file)).ToList();

        for (var i = lines.Count - 1; i >= 0; i--)
        {
            var text = lines[i].Trim();
            if (text.Contains(XmlLocalisationInterchangeFileFormatSunamo.cs))
            {
                var key = XmlLocalisationInterchangeFileFormatSunamo.GetConstsFromLine(text);
                var dx = remove.IndexOf(key);
                if (dx != -1)
                {
                    lines.RemoveAt(i);
                    remove.RemoveAt(dx);
                }
            }
        }

        //var d3 = lines.Where(d => d.Contains(d2));

        await File.WriteAllLinesAsync(file, lines);
        if (remove.Count > 0)
        {
            //throw new Exception( "Cant be deleted in XlfKeys: " + string.Join(",", remove));
        }
    }

    //public const string d2 = "YouCameToThisPageBecauseYouTriedToLoadThePageOrToPerformAnotherOperationThatYouDoNotHavePermissionToDoOrThatIsNotApplicableInThisContext";
    public static List<string> ParseConsts(List<string> lines, out int first)
    {
        var keys = new List<string>();
        first = -1;
        for (var i = 0; i < lines.Count; i++)
        {
            var text = lines[i].Trim();
            if (text.Contains(XmlLocalisationInterchangeFileFormatSunamo.cs))
            {
                if (first == -1) first = i;

                var key = XmlLocalisationInterchangeFileFormatSunamo.GetConstsFromLine(text);
                keys.Add(key);
            }
        }

        return keys;
    }

    public static List<string> ParseConstsAllLines(List<string> lines)
    {
        var keys = new List<string>();
        //first = -1;
        for (var i = 0; i < lines.Count; i++)
        {
            var text = lines[i].Trim();
            //if (text.Contains(CSharpParser.c))
            //{
            //if (first == -1)
            //{
            //    first = i;
            //}

            var key = XmlLocalisationInterchangeFileFormatSunamo.GetConstsFromLine(text);
            keys.Add(key);
            //}
        }

        return keys;
    }
}