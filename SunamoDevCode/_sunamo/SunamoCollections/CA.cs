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

    internal static List<string> StartingWith(string prefix, List<string> list)
    {
        for (var i = list.Count - 1; i >= 0; i--)
            if (!list[i].StartsWith(prefix))
                list.RemoveAt(i);
        return list;
    }

    internal static List<string> PostfixIfNotEnding(string prefix, List<string> list)
    {
        for (var i = 0; i < list.Count; i++)
            list[i] = prefix + list[i];
        return list;
    }

    internal static List<List<string>> Split(List<string> list, string delimiter)
    {
        var result = new List<List<string>>();
        var currentGroup = new List<string>();
        foreach (var item in list)
            if (item == delimiter)
            {
                result.Add(currentGroup);
                currentGroup.Clear();
            }

        return result;
    }

    internal static List<string> RemoveStringsEmptyTrimBefore(List<string> list)
    {
        for (var i = list.Count - 1; i >= 0; i--)
            if (list[i].Trim() == string.Empty)
                list.RemoveAt(i);
        return list;
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

    static string Replace(string text, string what, string replacement)
    {
        return text.Replace(what, replacement);
    }

    internal static void Replace(List<string> list, string what, string replacement)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = Replace(list[i], what, replacement);
        }
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

    internal static void RemoveStartingWith(string prefix, List<string> list, RemoveStartingWithArgs args = null)
    {
        if (args == null)
        {
            args = new RemoveStartingWithArgs();
        }

        var(isNegated, actualPrefix) = IsNegationTuple(prefix);
        prefix = actualPrefix;
        for (int i = list.Count - 1; i >= 0; i--)
        {
            var value = list[i];
            if (args.TrimBeforeFinding)
            {
                value = value.Trim();
            }

            if (isNegated)
            {
                if (!StartingWith(value, prefix, args.CaseSensitive))
                {
                    list.RemoveAt(i);
                }
            }
            else
            {
                if (StartingWith(value, prefix, args.CaseSensitive))
                {
                    list.RemoveAt(i);
                }
            }
        }
    }

    internal static bool StartingWith(string text, string prefix, bool isCaseSensitive)
    {
        if (isCaseSensitive)
        {
            return text.StartsWith(prefix);
        }
        else
        {
            return text.ToLower().StartsWith(prefix.ToLower());
        }
    }

    internal static string StartWith(List<string> prefixes, string text, out string matchedPrefix)
    {
        matchedPrefix = null;
        if (prefixes != null)
        {
            foreach (var prefix in prefixes)
            {
                if (text.StartsWith(prefix))
                {
                    matchedPrefix = prefix;
                    return text;
                }
            }
        }

        return null;
    }

    internal static void RemoveWhichContains(List<string> list, string pattern, bool isWildcard, Func<string, string, bool> wildcardIsMatch)
    {
        if (isWildcard)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (wildcardIsMatch(list[i], pattern))
                {
                    list.RemoveAt(i);
                }
            }
        }
        else
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].Contains(pattern))
                {
                    list.RemoveAt(i);
                }
            }
        }
    }
}