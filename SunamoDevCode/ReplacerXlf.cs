namespace SunamoDevCode;

/// <summary>
///     Dictionary as cache is good in database but not in ordinal c# app!
/// </summary>
public class ReplacerXlf
{
    private static ReplacerXlf? instance;
    private readonly List<string> values;
    /// <summary>
    /// Dictionary mapping original XLF keys to their versions with underscores removed.
    /// </summary>
    public Dictionary<string, string> WithWithoutUnderscore { get; set; } = new();

    private ReplacerXlf()
    {
        AllLists.InitHtmlEntitiesFullNames();

        values = AllLists.htmlEntitiesFullNames.Values.ToList();

        //values.Sort(SunamoComparer.StringLength.Instance.Desc);
        CA.Prepend("_", values);
    }

    /// <summary>
    /// Gets the singleton instance of ReplacerXlf, creating it if necessary.
    /// </summary>
    public static ReplacerXlf Instance
    {
        get
        {
            if (instance == null) instance = new ReplacerXlf();

            return instance;
        }
    }

    /// <summary>
    /// Removes HTML entity underscore suffixes from the given text.
    /// </summary>
    /// <param name="text">Text to clean up.</param>
    /// <returns>Text with underscore-prefixed HTML entity names removed.</returns>
    public string WithoutUnderscore(string text)
    {
        foreach (var item in values) text = text.Replace(item, string.Empty);
        return text;
    }

    //public static void AddKeys(List<string> k)
    //{
    //}

    //public static void AddKeysXlfKeysIds()
    //{
    //    List<string> ids = null; // XlfResourcesH.PathToXlfSunamo(Langs.en);
    //    var ids2 = new List<string>(ids);

    //    AddKeys(ids);
    //}
}