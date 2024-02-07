﻿using Diacritics.Extensions;

namespace SunamoDevCode._sunamo;
internal class SH
{
    internal static bool ContainsAtLeastOne(string p, List<string> aggregate)
    {
        foreach (var item in aggregate)
        {
            if (p.Contains(item))
            {
                return true;
            }
        }
        return false;
    }

    internal static string GetLineFromCharIndex(string content, List<string> lines, int dx2)
    {
        var dx = GetLineIndexFromCharIndex(content, dx2);
        return lines[dx];
    }

    /// <summary>
    /// Return index, therefore x-1
    /// </summary>
    /// <param name="input"></param>
    /// <param name="pos"></param>
    internal static int GetLineIndexFromCharIndex(string input, int pos)
    {
        var lineNumber = input.Take(pos).Count(c => c == '\n') + 1;
        return lineNumber - 1;
    }

    internal static string GetTextBetweenSimple(string p, string after, string before, bool throwExceptionIfNotContains = true)
    {
        int dxOfFounded = int.MinValue;
        var t = GetTextBetween(p, after, before, out dxOfFounded, 0, throwExceptionIfNotContains);
        return t;
    }

    internal static string GetTextBetween(string p, string after, string before, out int dxOfFounded, int startSearchingAt, bool throwExceptionIfNotContains = true)
    {
        string vr = null;
        dxOfFounded = p.IndexOf(after, startSearchingAt);
        int p3 = p.IndexOf(before, dxOfFounded + after.Length);
        bool b2 = dxOfFounded != -1;
        bool b3 = p3 != -1;
        if (b2 && b3)
        {
            dxOfFounded += after.Length;
            p3 -= 1;
            // When I return between ( ), there must be +1
            var length = p3 - dxOfFounded + 1;
            if (length < 1)
            {
                // Takhle to tu bylo předtím ale logicky je to nesmysl.
                //return p;
            }
            vr = p.Substring(dxOfFounded, length).Trim();
        }
        else
        {
            if (throwExceptionIfNotContains)
            {
                ThrowEx.NotContains(p, after, before);
            }
            else
            {
                // 24-1-21 return null instead of p
                return null;
                //vr = p;
            }
        }

        return vr.Trim();
    }

    internal static string WhiteSpaceFromStart(string v)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var item in v)
        {
            if (char.IsWhiteSpace(item))
            {
                sb.Append(item);
            }
            else
            {
                break;
            }
        }
        return sb.ToString();
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

    internal static void FirstCharUpper(ref string nazevPP)
    {
        nazevPP = FirstCharUpper(nazevPP);
    }


    internal static string FirstCharUpper(string nazevPP)
    {
        if (nazevPP.Length == 1)
        {
            return nazevPP.ToUpper();
        }

        string sb = nazevPP.Substring(1);
        return nazevPP[0].ToString().ToUpper() + sb;
    }

    internal static bool MatchWildcard(string name, string mask)
    {
        return IsMatchRegex(name, mask, AllChars.q, AllChars.asterisk);
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
        pat = pat.Replace(escapedSingle, AllStrings.dot);
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
        vcem = NormalizeString(vcem);
        List<int> Results = new List<int>();
        for (int Index = 0; Index < (vcem.Length - co.Length) + 1; Index++)
        {
            var subs = vcem.Substring(Index, co.Length);
            ////////DebugLogger.Instance.WriteLine(subs);
            // non-breaking space. &nbsp; code 160
            // 32 space
            char ch = subs[0];
            char ch2 = co[0];
            if (subs == AllStrings.space)
            {
            }
            if (subs == co)
                Results.Add(Index);
        }
        return Results;
    }

    internal static string NormalizeString(string s)
    {
        if (s.Contains(AllChars.nbsp))
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in s)
            {
                if (item == AllChars.nbsp)
                {
                    sb.Append(AllChars.space);
                }
                else
                {
                    sb.Append(item);
                }
            }
            return sb.ToString();
        }

        return s;
    }

    internal static string TextWithoutDiacritic(string projName)
    {
        return projName.RemoveDiacritics();
    }

    internal static bool ContainsDiacritic(string target)
    {
        return target.HasDiacritics();
    }
}
