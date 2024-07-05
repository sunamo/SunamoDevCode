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
    internal static string KeepAfterFirst(string searchQuery, string after, bool keepDeli = false)
    {
        var dx = searchQuery.IndexOf(after);
        if (dx != -1)
        {
            searchQuery = TrimStart(searchQuery.Substring(dx), after);
            if (keepDeli)
            {
                searchQuery = after + searchQuery;
            }
        }
        return searchQuery;
    }

    private static string TrimStart(string target, string trimString)
    {
        if (string.IsNullOrEmpty(trimString)) return target;

        string result = target;
        while (result.StartsWith(trimString))
        {
            result = result.Substring(trimString.Length);
        }

        return result;
    }



    internal static string RemoveAfterLast(string nameSolution, object delimiter)
    {
        int dex = nameSolution.LastIndexOf(delimiter.ToString());
        if (dex != -1)
        {
            string s = nameSolution.Substring(0, dex); //SHSubstring.Substring(, 0, dex, new SubstringArgs());
            return s;
        }
        return nameSolution;
    }
    internal static string RemoveAfterFirst(string t, char ch)
    {
        int dex = t.IndexOf(ch);
        return dex == -1 || dex == t.Length - 1 ? t : t.Substring(0, dex);
    }

}
