namespace SunamoDevCode._sunamo.SunamoCollections;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
internal partial class CA
{
    internal enum SearchStrategyCA
    {
        FixedSpace,
        AnySpaces,
        ExactlyName
    }

    internal static List<int> ReturnWhichContainsIndexes(IList<string> value, string term /*,
        SearchStrategyCA searchStrategy = SearchStrategyCA.FixedSpace*/)
    {
        var result = new List<int>();
        var i = 0;
        if (value != null)
            foreach (var item in value)
            {
                if (item.Contains(term) /*.Contains(item, term, searchStrategy)*/)
                    result.Add(i);
                i++;
            }

        return result;
    }

    internal static List<int> ReturnWhichContainsIndexes(string item, IList<string> terms /*,
       SearchStrategyCA searchStrategy = SearchStrategyCA.FixedSpace*/)
    {
        var result = new List<int>();
        var i = 0;
        foreach (var term in terms)
        {
            if (item.Contains(term) /*.Contains(item, term, searchStrategy)*/)
                result.Add(i);
            i++;
        }

        return result;
    }

    internal static IList<int> ReturnWhichContainsIndexes(IList<string> parts, IList<string> mustContains)
    {
        var result = new List<int>();
        foreach (var item in mustContains)
            result.AddRange(ReturnWhichContainsIndexes(parts, item));
        result = result.Distinct().ToList();
        return result;
    }

    internal static List<string> StartingWith(string v, List<string> list)
    {
        for (var i = list.Count - 1; i >= 0; i--)
            if (!list[i].StartsWith(v))
                list.RemoveAt(i);
        return list;
    }

    internal static List<string> PostfixIfNotEnding(string pre, List<string> list)
    {
        for (var i = 0; i < list.Count; i++)
            list[i] = pre + list[i];
        return list;
    }

    internal static List<List<string>> Split(List<string> text, string determining)
    {
        var sourceList = new List<List<string>>();
        var actual = new List<string>();
        foreach (var item in text)
            if (item == determining)
            {
                sourceList.Add(actual);
                actual.Clear();
            }

        return sourceList;
    }

    internal static List<string> RemoveStringsEmptyTrimBefore(List<string> mySites)
    {
        for (var i = mySites.Count - 1; i >= 0; i--)
            if (mySites[i].Trim() == string.Empty)
                mySites.RemoveAt(i);
        return mySites;
    }

    internal static bool ContainsAnyFromElementBool(string text, IList<string> list /*,
        bool acceptAsteriskForPassingAll = false*/)
    {
        if (list.Count == 1 && list.First() == "*")
            return true;
        var result = new List<int>();
        foreach (var item in list)
            if (text.Contains(item))
                return true;
        return false;
    }

    internal static void RemoveNullEmptyWs(List<string> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (string.IsNullOrWhiteSpace(list[i]))
            {
                list.RemoveAt(i);
            }
        }
    }

    /// <summary>
    ///     Direct edit input collection
    /// </summary>
    /// <param name = "l"></param>
    internal static List<string> Trim(List<string> list)
    {
        for (var i = 0; i < list.Count; i++)
            list[i] = list[i].Trim();
        return list;
    }

    internal static void DoubleOrMoreMultiLinesToSingle(ref string list)
    {
        var name = Environment.NewLine;
        list = Regex.Replace(list, @"(\r?\n\s*){2,}", Environment.NewLine + Environment.NewLine);
        list = list.Trim();
    //list = list.Replace(name, name + name);
    // 27-10-23 dříve to bylo takhle
    //return list.Trim();
    }

    internal static void TrimWhereIsOnlyWhitespace(List<string> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            var item = list[i];
            if (string.IsNullOrWhiteSpace(item))
            {
                list[i] = list[i].Trim();
            }
        }
    }

    static string Replace(string text, string from, string to)
    {
        return text.Replace(from, to);
    }

    internal static void Replace(List<string> files_in, string what, string forWhat)
    {
        for (int i = 0; i < files_in.Count; i++)
        {
            files_in[i] = Replace(files_in[i], what, forWhat);
        }
    //CAChangeContent.ChangeContent2(null, files_in, Replace, what, forWhat);
    }

    internal static (bool, string) IsNegationTuple(string contains)
    {
        if (contains[0] == '!')
        {
            contains = contains.Substring(1);
            return (true, contains);
        }

        return (false, contains);
    }

    internal static void RemoveStartingWith(string start, List<string> mySites, RemoveStartingWithArgs a = null)
    {
        if (a == null)
        {
            a = new RemoveStartingWithArgs();
        }

        var(negate, start2) = IsNegationTuple(start);
        start = start2;
        for (int i = mySites.Count - 1; i >= 0; i--)
        {
            var val = mySites[i];
            if (a._trimBeforeFinding)
            {
                val = val.Trim();
            }

            if (negate)
            {
                if (!StartingWith(val, start, a.caseSensitive))
                {
                    mySites.RemoveAt(i);
                }
            }
            else
            {
                if (StartingWith(val, start, a.caseSensitive))
                {
                    mySites.RemoveAt(i);
                }
            }
        }
    }

    internal static bool StartingWith(string val, string start, bool caseSensitive)
    {
        if (caseSensitive)
        {
            return val.StartsWith(start);
        }
        else
        {
            return val.ToLower().StartsWith(start.ToLower());
        }
    }

    internal static string StartWith(List<string> suMethods, string line, out string element)
    {
        element = null;
        if (suMethods != null)
        {
            foreach (var method in suMethods)
            {
                if (line.StartsWith(method))
                {
                    element = method;
                    return line;
                }
            }
        }

        return null;
    }

    internal static void RemoveWhichContains(List<string> files1, string item, bool wildcard, Func<string, string, bool> WildcardIsMatch)
    {
        if (wildcard)
        {
            //item = SH.WrapWith(item, '*');
            for (int i = files1.Count - 1; i >= 0; i--)
            {
                if (WildcardIsMatch(files1[i], item))
                {
                    files1.RemoveAt(i);
                }
            }
        }
        else
        {
            for (int i = files1.Count - 1; i >= 0; i--)
            {
                if (files1[i].Contains(item))
                {
                    files1.RemoveAt(i);
                }
            }
        }
    }
}