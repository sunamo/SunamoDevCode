namespace SunamoDevCode._sunamo.SunamoString;

internal class SH
{
    #region SH.FirstCharUpper


    #endregion
    internal static bool IsContained(string text, string pattern)
    {
        var (isNegated, actualPattern) = IsNegationTuple(pattern);
        pattern = actualPattern;

        if (isNegated && text.Contains(pattern))
            return false;
        if (!isNegated && !text.Contains(pattern)) return false;

        return true;
    }

    internal static (bool, string) IsNegationTuple(string pattern)
    {
        if (pattern[0] == '!')
        {
            pattern = pattern.Substring(1);
            return (true, pattern);
        }

        return (false, pattern);
    }

    internal static int OccurencesOfStringIn(string source, string searchText)
    {
        return source.Split(new[] { searchText }, StringSplitOptions.None).Length - 1;
    }

    internal static bool ContainsAtLeastOne(string text, List<string> list)
    {
        foreach (var item in list)
        {
            if (text.Contains(item))
            {
                return true;
            }
        }
        return false;
    }

    internal static string GetLineFromCharIndex(string content, List<string> lines, int characterIndex)
    {
        var lineIndex = GetLineIndexFromCharIndex(content, characterIndex);
        return lines[lineIndex];
    }

    /// <summary>
    /// EN: Return index, therefore x-1
    /// CZ: Vrátí index, proto x-1
    /// </summary>
    internal static int GetLineIndexFromCharIndex(string text, int characterPosition)
    {
        var lineNumber = text.Take(characterPosition).Count(character => character == '\n') + 1;
        return lineNumber - 1;
    }

    internal static string GetTextBetweenSimple(string text, string afterDelimiter, string beforeDelimiter, bool isThrowExceptionIfNotContains = true)
    {
        int foundIndex = int.MinValue;
        var result = GetTextBetween(text, afterDelimiter, beforeDelimiter, out foundIndex, 0, isThrowExceptionIfNotContains);
        return result;
    }

    internal static string GetTextBetween(string text, string afterDelimiter, string beforeDelimiter, out int foundIndex, int startSearchingAt, bool isThrowExceptionIfNotContains = true)
    {
        string result = null;
        foundIndex = text.IndexOf(afterDelimiter, startSearchingAt);
        int beforeIndex = text.IndexOf(beforeDelimiter, foundIndex + afterDelimiter.Length);
        bool isAfterFound = foundIndex != -1;
        bool isBeforeFound = beforeIndex != -1;
        if (isAfterFound && isBeforeFound)
        {
            foundIndex += afterDelimiter.Length;
            beforeIndex -= 1;
            // When I return between ( ), there must be +1
            var length = beforeIndex - foundIndex + 1;
            if (length < 1)
            {
                // EN: This was here before but logically it's nonsense
                // CZ: Takhle to tu bylo předtím ale logicky je to nesmysl
            }
            result = text.Substring(foundIndex, length).Trim();
        }
        else
        {
            if (isThrowExceptionIfNotContains)
            {
                ThrowEx.NotContains(text, afterDelimiter, beforeDelimiter);
            }
            else
            {
                // 24-1-21 return null instead of text
                return null;
                //result = text;
            }
        }

        return result;
    }

    internal static string WhiteSpaceFromStart(string v)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (var item in v)
        {
            if (char.IsWhiteSpace(item))
            {
                stringBuilder.Append(item);
            }
            else
            {
                break;
            }
        }
        return stringBuilder.ToString();
    }

    internal static string PrefixIfNotStartedWith(string item, string http, bool skipWhitespaces = false)
    {
        string whitespaces = string.Empty;

        if (skipWhitespaces)
        {
            whitespaces = WhiteSpaceFromStart(item);
            item = item.Substring(whitespaces.Length);
        }

        if (!item.StartsWith(http))
        {
            return whitespaces + http + item;
        }

        return whitespaces + item;
    }

    internal static string WrapWith(string value, string h)
    {
        return h + value + h;
    }

    internal static string WrapWithQm(string value)
    {
        var h = "\"";
        return h + value + h;
    }

    internal static string WrapWithBs(string value)
    {
        var h = "\\";
        return h + value + h;
    }

    #region SH.FirstCharUpper
    internal static string FirstCharUpper(ref string nazevPP)
    {
        nazevPP = FirstCharUpper(nazevPP);
        return nazevPP;
    }


    internal static string FirstCharUpper(string nazevPP)
    {
        if (nazevPP.Length == 1)
        {
            return nazevPP.ToUpper();
        }

        string stringBuilder = nazevPP.Substring(1);
        return nazevPP[0].ToString().ToUpper() + stringBuilder;
    }
    #endregion

    internal static bool MatchWildcard(string name, string mask)
    {
        return IsMatchRegex(name, mask, '?', '*');
    }

    private static bool IsMatchRegex(string str, string pat, char singleWildcard, char multipleWildcard)
    {
        // If I compared .vs with .vs, return false before
        if (str == pat)
        {
            return true;
        }

        string escapedSingle = Regex.Escape(new string(singleWildcard, 1));
        string escapedMultiple = Regex.Escape(new string(multipleWildcard, 1));
        pat = Regex.Escape(pat);
        pat = pat.Replace(escapedSingle, ".");
        pat = "^" + pat.Replace(escapedMultiple, ".*") + "$";
        Regex reg = new Regex(pat);
        return reg.IsMatch(str);
    }


    internal static (string, string) GetPartsByLocationNoOut(string text, char or)
    {
        GetPartsByLocation(out var pred, out var za, text, or);
        return (pred, za);
    }

    internal static void GetPartsByLocation(out string pred, out string za, string text, char or)
    {
        int dex = text.IndexOf(or);
        GetPartsByLocation(out pred, out za, text, dex);
    }

    internal static void GetPartsByLocation(out string pred, out string za, string text, int pozice)
    {
        if (pozice == -1)
        {
            pred = text;
            za = "";
        }
        else
        {
            pred = text.Substring(0, pozice);
            if (text.Length > pozice + 1)
            {
                za = text.Substring(pozice + 1);
            }
            else
            {
                za = string.Empty;
            }
        }
    }

    internal static List<int> ReturnOccurencesOfString(string vcem, string co)
    {

        List<int> Results = new List<int>();
        for (int Index = 0; Index < (vcem.Length - co.Length) + 1; Index++)
        {
            var subs = vcem.Substring(Index, co.Length);
            ////////DebugLogger.Instance.WriteLine(subs);
            // non-breaking space. &nbsp; code 160
            // 32 space
            char ch = subs[0];
            char ch2 = co[0];
            if (subs == "")
            {
            }
            if (subs == co)
                Results.Add(Index);
        }
        return Results;
    }


    internal static string TextWithoutDiacritic(string projName)
    {
        return projName.RemoveDiacritics();
    }

    internal static bool ContainsDiacritic(string target)
    {
        return target.HasDiacritics();
    }

    internal static string GetTextBetween(string content, string start, string end, bool throwExceptionIfNotContains = true)
    {
        return GetTextBetweenSimple(content, start, end, throwExceptionIfNotContains);
    }
}