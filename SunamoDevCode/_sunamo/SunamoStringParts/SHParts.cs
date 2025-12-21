namespace SunamoDevCode._sunamo.SunamoStringParts;

internal class SHParts
{


    internal static string RemoveAfterFirst(string t, string ch)
    {
        int dex = t.IndexOf(ch);
        if (dex == -1 || dex == t.Length - 1)
        {
            return t;
        }

        string vr = t.Remove(dex);
        return vr;
    }
    // Metoda KeepAfterFirst byla odstraněna - inlined v TypeScriptHelper.cs:79



    // Metoda RemoveAfterLast byla odstraněna - inlined v SolutionFolderSerialize.cs:57
    internal static string RemoveAfterFirst(string t, char ch)
    {
        int dex = t.IndexOf(ch);
        return dex == -1 || dex == t.Length - 1 ? t : t.Substring(0, dex);
    }

}