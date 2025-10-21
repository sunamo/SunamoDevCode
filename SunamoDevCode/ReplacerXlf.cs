// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode;

/// <summary>
///     Dictionary as cache is good in database but not in ordinal c# app!
/// </summary>
public class ReplacerXlf
{
    private static ReplacerXlf instance;
    private readonly List<string> val;
    public Dictionary<string, string> withWithoutUnderscore = new();

    private ReplacerXlf()
    {
        AllLists.InitHtmlEntitiesFullNames();

        val = AllLists.htmlEntitiesFullNames.Values.ToList();

        //val.Sort(SunamoComparer.StringLength.Instance.Desc);
        CA.Prepend("_", val);
    }

    public static ReplacerXlf Instance
    {
        get
        {
            if (instance == null) instance = new ReplacerXlf();

            return instance;
        }
    }

    public string WithoutUnderscore(string s)
    {
        foreach (var item2 in val) s = s.Replace(item2, string.Empty);
        return s;
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