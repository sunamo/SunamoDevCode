using SunamoCollectionsIndexesWithNull;

namespace SunamoDevCode;


public partial class CSharpParser
{
    static Type type = typeof(CSharpParser);
    //public const string c = "const string";
    public static string c => XmlLocalisationInterchangeFileFormatSunamo.cs;
    public const string p = "public ";

    /// <summary>
    /// Directly save to file
    /// In A2 will be what can't be deleted, when will be > 0, ThrowException
    /// </summary>
    /// <param name = "file"></param>
    /// <param name = "remove"></param>
    public static
#if ASYNC
    async Task
#else
    void
#endif
 RemoveConsts(string file, List<string> remove)
    {
        remove.Leading(null);

        var ind = CAIndexesWithNull.IndexesWithNull(remove);

        var lines = (
#if ASYNC
    await
#endif
 File.ReadAllLinesAsync(file)).ToList();

        for (int i = lines.Count - 1; i >= 0; i--)
        {
            var text = lines[i].Trim();
            if (text.Contains(CSharpParser.c))
            {
                string key = XmlLocalisationInterchangeFileFormatSunamo.GetConstsFromLine(text);
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
}
