namespace SunamoDevCode;

/// <summary>
/// Manager for working with constant definitions in XlfKeys.cs file
/// </summary>
public class ConstsManager
{
    /// <summary>
    /// Path to XlfKeys.cs file
    /// </summary>
    public string PathXlfKeys { get; }

    #region Work with consts in XlfKeys

    /// <summary>
    /// Add to XlfKeys.cs from xlf
    /// Must manually call XlfResourcesH.SaveResouresToRL(BasePathsHelper.sunamoProject) before
    /// Called externally from MiAddTranslationWhichIsntInKeys_Click
    /// </summary>
    /// <param name="keysAll">List of keys to add</param>
    /// <param name="valuesAll">Optional list of values corresponding to keys. If null, keys will be used as values.</param>
    public
#if ASYNC
        async Task
#else
    void
#endif
        AddConsts(List<string> keysAll, List<string>? valuesAll = null)
    {
        var keys =
#if ASYNC
            await
#endif
                GetConsts();

        AddKeysConsts(keysAll, keys.Item2, keys.Item3, valuesAll);
    }


    /// <summary>
    /// Adds C# const code to the generator
    /// </summary>
    /// <param name="generator">CSharp code generator instance</param>
    /// <param name="constantName">Name of the constant to add</param>
    /// <param name="constantValue">Value of the constant</param>
    private static void AddConst(CSharpGenerator generator, string constantName, string constantValue)
    {
        generator.Field(1, AccessModifiers.Public, true, VariableModifiers.Mapped, "string", constantName, true, constantValue);
    }


    /// <summary>
    /// Get consts which exists in XlfKeys.cs
    /// </summary>
    /// <returns>Tuple containing keys, first line index, and all lines</returns>
    public
#if ASYNC
        async Task<OutRef3DC<List<string>, int, List<string>>>
#else
      OutRef3DC<List<string>, int, List<string>>
#endif
        GetConsts()
    {
        var first = -1;

        var lines = SHGetLines.GetLines(
#if ASYNC
            await
#endif
                File.ReadAllTextAsync(PathXlfKeys)).ToList();

        var keys = CSharpParser.ParseConsts(lines, out first);
        return new OutRef3DC<List<string>, int, List<string>>(keys, first, lines);
    }

    /// <summary>
    /// Initializes a new instance of ConstsManager
    /// </summary>
    /// <param name="pathXlfKeys">Path to XlfKeys.cs file</param>
    /// <param name="shouldIncludeInXlfKeys">Function to determine if key should be in XlfKeys</param>
    public ConstsManager(string pathXlfKeys, Func<string, bool> shouldIncludeInXlfKeys)
    {
        PathXlfKeys = pathXlfKeys;
        this.shouldIncludeInXlfKeys = shouldIncludeInXlfKeys;
    }

    private readonly Func<string, bool> shouldIncludeInXlfKeys;

    /// <summary>
    /// Add constant keys to the generated code
    /// </summary>
    /// <param name="keysAll">List of all keys to add</param>
    /// <param name="first">Position where to insert the generated code</param>
    /// <param name="lines">List of source code lines to modify</param>
    /// <param name="valuesAll">Optional list of values. If null, keys will be used as values.</param>
    public void AddKeysConsts(List<string> keysAll, int first, List<string> lines, List<string>? valuesAll = null)
    {
        var generator = new CSharpGenerator();

        var append = string.Empty;

        if (valuesAll == null)
        {
            valuesAll = keysAll;
        }
        else
        {
            if (valuesAll.Count != keysAll.Count)
                ThrowEx.DifferentCountInLists("keysAll", keysAll, "valuesAll", valuesAll);
        }

        for (var i = 0; i < keysAll.Count; i++)
        {
            var key = keysAll[i];
            var constantValue = valuesAll[i];

            if (shouldIncludeInXlfKeys(key))
            {
                append = string.Empty;

                if (char.IsDigit(key[0])) append = "_";

                // Inline from SHTrim.TrimLeadingNumbersAtStart - removes digits from start of string
                var trimmedKey = key;
                while (trimmedKey.Length > 0 && char.IsDigit(trimmedKey[0]))
                {
                    trimmedKey = trimmedKey.Substring(1);
                }
                AddConst(generator, append + trimmedKey, constantValue);
            }
        }

        lines.Insert(first, generator.ToString());


        CA.RemoveStringsEmpty2(lines);

        File.WriteAllLinesAsync(PathXlfKeys, lines);
    }

    #endregion
}
