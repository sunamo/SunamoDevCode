namespace SunamoDevCode.Helpers;

public class TypeScriptHelper
{
    static Dictionary<string, string> types = new Dictionary<string, string>();
    static Dictionary<string, string> defaultValueForType = new Dictionary<string, string>();


    static TypeScriptHelper()
    {

        //types.Add("string", "string2");
        //types.Add("boolean", "boolean2");

        defaultValueForType.Add("string", "\"\"");
        defaultValueForType.Add("number", "0");
        defaultValueForType.Add("boolean", "false");
        defaultValueForType.Add("Date", "dt");
    }

    public static string Type(string d)
    {
        if (types.ContainsKey(d))
        {
            return types[d];
        }

        return d;
    }

    public static string DefaultValueForType(string t, string prefixIfString = "", /*bool isArgNumber = false,*/ string nameArgMethod = "")
    {
        if (t.EndsWith("[]"))
        {
            return "[]";
        }

        if (defaultValueForType.ContainsKey(t))
        {
            var result = defaultValueForType[t];
            if (t == "string")
            {
                result = result.Insert(1, prefixIfString);
                if (nameArgMethod != "")
                {
                    result += " + " + nameArgMethod;
                }
            }
            else if (t == "number")
            {
                if (nameArgMethod != "")
                {
                    result = "+" + nameArgMethod;
                }
            }

            return result;
        }

        ThrowEx.NotImplementedCase(t);
        return "";
    }


    /// <summary>
    /// Nepoužívat toto na interfacy, v těch nesmím mít ?, kromě případů kdy to explicitně povolím
    /// </summary>
    /// <param name="l"></param>
    /// <returns></returns>
    public static Tuple<List<string>, List<string>> GetNamesAndTypes(List<string> l)
    {
        var l2 = l.ToList();

        CAChangeContent.ChangeContent(new ChangeContentArgsDC { }, l, SHParts.RemoveAfterFirst, ':');
        CA.Trim(l);
        CA.TrimEnd(l, '?');
        CA.Trim(l);

        // Inlined from SHParts.KeepAfterFirst - ponechává text za prvním výskytem znaku
        Func<string, string, bool, string> keepAfterFirst = (searchQuery, after, keepDeli) =>
        {
            var dx = searchQuery.IndexOf(after);
            if (dx != -1)
            {
                // TrimStart helper - odstraňuje řetězec ze začátku
                string result = searchQuery.Substring(dx);
                while (result.StartsWith(after))
                {
                    result = result.Substring(after.Length);
                }
                searchQuery = result;
                if (keepDeli)
                {
                    searchQuery = after + searchQuery;
                }
            }
            return searchQuery;
        };
        CAChangeContent.ChangeContent(new ChangeContentArgsDC { }, l2, keepAfterFirst, ":", false);
        for (int i = 0; i < l2.Count; i++)
        {
            var t = l2[i];
            t = t.Trim();
            t = t.TrimEnd(';');
            t = t.Trim('2');
            l2[i] = t;
        }

        return new Tuple<List<string>, List<string>>(l, l2);
    }
}
