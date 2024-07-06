namespace SunamoDevCode;


public class ConstsManager
{
    /// <summary>
    /// XlfKeys.cs
    /// </summary>
    public readonly string pathXlfKeys = null;

    #region Work with consts in XlfKeys
    /// <summary>
    /// Add to XlfKeys.cs from xlf
    /// Must manually call XlfResourcesH.SaveResouresToRL(BasePathsHelper.sunamoProject) before
    /// called externally from MiAddTranslationWhichIsntInKeys_Click
    /// </summary>
    /// <param name="keysAll"></param>
    public
#if ASYNC
    async Task
#else
    void
#endif
 AddConsts(List<string> keysAll, List<string> valuesAll = null)
    {
        int first = -1;

        List<string> lines = null;
        var keys =
#if ASYNC
    await
#endif
 GetConsts();

        var both = CAG.CompareList(keys.Item1, keysAll);
        AddKeysConsts(keysAll, first, lines, valuesAll);
    }


    /// <summary>
    /// Add c# const code
    /// </summary>
    /// <param name="csg"></param>
    /// <param name="item"></param>
    private static void AddConst(CSharpGenerator csg, string item, string val)
    {
        csg.Field(1, AccessModifiers.Public, true, VariableModifiers.Mapped, "string", item, true, val);
    }


    /// <summary>
    /// Get consts which exists in XlfKeys.cs
    /// </summary>
    /// <param name="first"></param>
    /// <param name="lines"></param>
    public
#if ASYNC
    async Task<OutRef3DC<List<string>, int, List<string>>>
#else
      OutRef<List<string>, int, List<string>>
#endif
 GetConsts()
    {
        var first = -1;

        var lines = SHGetLines.GetLines (
#if ASYNC
    await
#endif
 File.ReadAllTextAsync(pathXlfKeys)).ToList();

        var keys = CSharpParser.ParseConsts(lines, out first);
        return new OutRef3DC<List<string>, int, List<string>>(keys, first, lines);
    }

    public ConstsManager(string pathXlfKeys, Func<string, bool> XmlLocalisationInterchangeFileFormatIsToBeInXlfKeys)
    {
        this.pathXlfKeys = pathXlfKeys;
        this.XmlLocalisationInterchangeFileFormatIsToBeInXlfKeys = XmlLocalisationInterchangeFileFormatIsToBeInXlfKeys;
    }

    Func<string, bool> XmlLocalisationInterchangeFileFormatIsToBeInXlfKeys;
    static Type type = typeof(ConstsManager);

    public void AddKeysConsts(List<string> keysAll, int first, List<string> lines, List<string> valuesAll = null)
    {
        CSharpGenerator csg = new CSharpGenerator();

        string append = string.Empty;

        if (valuesAll == null)
        {
            valuesAll = keysAll;
        }
        else
        {
            if (valuesAll.Count != keysAll.Count)
            {
                ThrowEx.DifferentCountInLists("keysAll", keysAll, "valuesAll", valuesAll);
            }
        }

        for (int i = 0; i < keysAll.Count; i++)
        {
            var item = keysAll[i];
            var val = valuesAll[i];

            if (XmlLocalisationInterchangeFileFormatIsToBeInXlfKeys(item))
            {
                append = string.Empty;

                if (char.IsDigit(item[0]))
                {
                    append = "_";
                }

                AddConst(csg, append + SHTrim.TrimLeadingNumbersAtStart(item), val);
            }
        }

        lines.Insert(first, csg.ToString());



        CA.RemoveStringsEmpty2(lines);

        File.WriteAllLinesAsync(pathXlfKeys, lines);
    }
    #endregion
}
