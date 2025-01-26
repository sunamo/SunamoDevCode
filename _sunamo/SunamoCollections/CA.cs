namespace SunamoDevCode._sunamo.SunamoCollections;

internal class CA
{
    public static List<string> StartingWith(string v, List<string> l)
    {
        for (var i = l.Count - 1; i >= 0; i--)
            if (!l[i].StartsWith(v))
                l.RemoveAt(i);
        return l;
    }
    public static List<string> PostfixIfNotEnding(string pre, List<string> l)
    {
        for (var i = 0; i < l.Count; i++) l[i] = pre + l[i];
        return l;
    }
    public static List<List<string>> Split(List<string> s, string determining)
    {
        var ls = new List<List<string>>();
        var actual = new List<string>();
        foreach (var item in s)
            if (item == determining)
            {
                ls.Add(actual);
                actual.Clear();
            }

        return ls;
    }

    internal static List<string> RemoveStringsEmptyTrimBefore(List<string> mySites)
    {
        for (var i = mySites.Count - 1; i >= 0; i--)
            if (mySites[i].Trim() == string.Empty)
                mySites.RemoveAt(i);
        return mySites;
    }


    public static bool ContainsAnyFromElementBool(string s, IList<string> list,
        bool acceptAsteriskForPassingAll = false)
    {
        if (list.Count == 1 && list.First() == "*") return true;

        var result = new List<int>();

        foreach (var item in list)
            if (s.Contains(item))
                return true;

        return false;
    }

    internal static void RemoveNullEmptyWs(List<string> l)
    {
        for (int i = l.Count - 1; i >= 0; i--)
        {
            if (string.IsNullOrWhiteSpace(l[i]))
            {
                l.RemoveAt(i);
            }
        }
    }

    internal static void InitFillWith<T>(List<T> datas, int pocet, T initWith)
    {
        for (int i = 0; i < pocet; i++)
        {
            datas.Add(initWith);
        }
    }
    /// <summary>
    ///     Usage: IEnumerableExtensions
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    internal static int Count(IEnumerable e)
    {
        if (e == null) return 0;
        var t = e.GetType();
        var tName = t.Name;
        // nevím jak to má .net core, zatím tu ThreadHelper nebudu přesouvat
        // if (ThreadHelper.NeedDispatcher(tName))
        // {
        //     int result = dCountSunExc(e);
        //     return result;
        // }
        if (e is IList) return (e as IList).Count;
        if (e is Array) return (e as Array).Length;
        var count = 0;
        foreach (var item in e) count++;
        return count;
    }
    /// <summary>
    ///     Direct edit input collection
    /// </summary>
    /// <param name="l"></param>
    internal static List<string> Trim(List<string> l)
    {
        for (var i = 0; i < l.Count; i++) l[i] = l[i].Trim();
        return l;
    }
    internal static string First(IEnumerable v2)
    {
        foreach (var item in v2) return item.ToString();
        return null;
    }


    internal static void DoubleOrMoreMultiLinesToSingle(ref string list)
    {
        var n = Environment.NewLine;
        list = Regex.Replace(list, @"(\r?\n\s*){2,}", Environment.NewLine + Environment.NewLine);
        list = list.Trim();
        //list = list.Replace(n, n + n);
        // 27-10-23 dříve to bylo takhle
        //return list.Trim();
    }
    internal static void TrimWhereIsOnlyWhitespace(List<string> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            var l = list[i];
            if (string.IsNullOrWhiteSpace(l))
            {
                list[i] = list[i].Trim();
            }
        }
    }

    static string Replace(string s, string from, string to)
    {
        return s.Replace(from, to);
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

        var (negate, start2) = IsNegationTuple(start);
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

    internal static void RemoveWhichContainsList(List<string> files, List<string> list, bool wildcard, Func<string, string, bool> WildcardIsMatch = null)
    {
        foreach (var item in list)
        {
            RemoveWhichContains(files, item, wildcard, WildcardIsMatch);
        }
    }

    internal static List<string> TrimEnd(List<string> sf, params char[] toTrim)
    {
        for (int i = 0; i < sf.Count; i++)
        {
            sf[i] = sf[i].TrimEnd(toTrim);
        }
        return sf;
    }

    internal static List<T> JoinIList<T>(params IList<T>[] enumerable)
    {
        List<T> t = new List<T>();
        foreach (var item in enumerable)
        {
            foreach (var item2 in item)
            {
                t.Add((T)item2);
            }
        }
        return t;
    }

    internal static void RemoveEmptyLinesToFirstNonEmpty(List<string> content)
    {
        for (int i = 0; i < content.Count; i++)
        {
            var line = content[i];
            if (line.Trim() == string.Empty)
            {
                content.RemoveAt(i);
                i--;
            }
            else
            {
                break;
            }
        }
    }

    internal static void RemoveLines(List<string> lines, List<int> removeLines)
    {
        removeLines.Sort();
        for (int i = removeLines.Count - 1; i >= 0; i--)
        {
            var dx = removeLines[i];
            lines.RemoveAt(dx);
        }
    }

    internal static List<string> RemoveStringsEmpty2(List<string> mySites)
    {
        for (int i = mySites.Count - 1; i >= 0; i--)
        {
            if (mySites[i].Trim() == string.Empty)
            {
                mySites.RemoveAt(i);
            }
        }
        return mySites;
    }



    internal static List<string> WrapWith(List<string> whereIsUsed2, string v)
    {
        return WrapWith(whereIsUsed2, v, v);
    }

    /// <summary>
    /// direct edit
    /// </summary>
    /// <param name="whereIsUsed2"></param>
    /// <param name="v"></param>
    internal static List<string> WrapWith(List<string> whereIsUsed2, string before, string after)
    {
        for (int i = 0; i < whereIsUsed2.Count; i++)
        {
            whereIsUsed2[i] = before + whereIsUsed2[i] + after;
        }
        return whereIsUsed2;
    }

    internal static List<string> EnsureBackslash(List<string> eb)
    {
        for (int i = 0; i < eb.Count; i++)
        {
            string r = eb[i];
            if (r[r.Length - 1] != '\\')
            {
                eb[i] = r + "\\";
            }
        }

        return eb;
    }

    internal static bool ContainsElement<T>(IList<T> list, T t)
    {
        if (list.Count == 0)
        {
            return false;
        }
        foreach (T item in list)
        {
            if (Comparer<T>.Equals(item, t))
            {
                return true;
            }
        }

        return false;
    }

    internal static void RemoveWildcard(List<string> d, string mask)
    {
        //https://stackoverflow.com/a/15275806

        for (int i = d.Count - 1; i >= 0; i--)
        {
            if (SH.MatchWildcard(d[i], mask))
            {
                d.RemoveAt(i);
            }
        }
    }

    internal static bool HasPostfix(string key, params string[] v1)
    {
        foreach (var item in v1)
        {
            if (key.EndsWith(item))
            {
                return true;
            }
        }
        return false;
    }

    internal static List<string> Prepend(string v, List<string> toReplace)
    {
        for (int i = 0; i < toReplace.Count; i++)
        {
            if (!toReplace[i].StartsWith(v))
            {
                toReplace[i] = v + toReplace[i];
            }
        }
        return toReplace;
    }

    internal static List<string> ToListString(params string[] v)
    {
        return v.ToList();
    }
}