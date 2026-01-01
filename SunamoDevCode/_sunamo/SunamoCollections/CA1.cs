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

    internal static List<T> JoinIList<T>(params IList<T>[] lists)
    {
        List<T> result = new List<T>();
        foreach (var list in lists)
        {
            foreach (var element in list)
            {
                result.Add((T)element);
            }
        }

        return result;
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

    internal static void RemoveLines(List<string> lines, List<int> lineIndexesToRemove)
    {
        lineIndexesToRemove.Sort();
        for (int i = lineIndexesToRemove.Count - 1; i >= 0; i--)
        {
            var lineIndex = lineIndexesToRemove[i];
            lines.RemoveAt(lineIndex);
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

    internal static List<string> WrapWith(List<string> list, string wrapText)
    {
        return WrapWith(list, wrapText, wrapText);
    }

    /// <summary>
    /// EN: Direct edit of list
    /// CZ: Přímá editace listu
    /// </summary>
    internal static List<string> WrapWith(List<string> list, string prefixText, string suffixText)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = prefixText + list[i] + suffixText;
        }

        return list;
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

    internal static List<string> Prepend(string prefix, List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].StartsWith(prefix))
            {
                list[i] = prefix + list[i];
            }
        }

        return list;
    }

    internal static List<string> ToListString(params string[] values)
    {
        return values.ToList();
    }
}