namespace SunamoDevCode._sunamo.SunamoCollections;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
internal partial class CA
{
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
    /// <param name = "whereIsUsed2"></param>
    /// <param name = "v"></param>
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