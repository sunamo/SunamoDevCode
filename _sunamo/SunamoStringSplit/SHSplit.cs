namespace SunamoDevCode._sunamo.SunamoStringSplit;

internal class SHSplit
{
    internal static List<string> Split(string p, params string[] newLine)
    {
        return p.Split(newLine, StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    /// <summary>
    ///     Get null if count of getted parts was under A2.
    ///     Automatically add empty padding items at end if got lower than A2
    ///     Automatically join overloaded items to last by A2.
    /// </summary>
    /// <param name="p"></param>
    /// <param name="p_2"></param>
    public static List<string> SplitToParts(string what, int parts, string deli)
    {
        var s = Split(what.RemoveInvisibleChars(), deli);
        if (s.Count < parts)
        {
            // Pokud je pocet ziskanych partu mensi, vlozim do zbytku prazdne retezce
            if (s.Count > 0)
            {
                var vr2 = new List<string>();
                for (var i = 0; i < parts; i++)
                    if (i < s.Count)
                        vr2.Add(s[i]);
                    else
                        vr2.Add("");
                return vr2;
                //return new string[] { s[0] };
            }

            return null;
        }

        if (s.Count == parts)
            // Pokud pocet ziskanych partu souhlasim presne, vratim jak je
            return s;
        // Pokud je pocet ziskanych partu vetsi nez kolik ma byt, pripojim ty co josu navic do zbytku
        parts--;
        var vr = new List<string>();
        for (var i = 0; i < s.Count; i++)
            if (i < parts)
                vr.Add(s[i]);
            else if (i == parts)
                vr.Add(s[i] + deli);
            else if (i != s.Count - 1)
                vr[parts] += s[i] + deli;
            else
                vr[parts] += s[i];
        return vr;
    }

    internal static List<string> SplitChar(string parametry, char deli)
    {
        return Split(StringSplitOptions.RemoveEmptyEntries, parametry, (new List<char>([deli]).ConvertAll(d => d.ToString()).ToArray()));
    }

    internal static List<string> Split(StringSplitOptions stringSplitOptions, string text, params string[] deli)
    {
        if (deli == null || deli.Length == 0)
        {
            throw new Exception("NoDelimiterDetermined");
        }
        //var ie = CA.OneElementCollectionToMulti(deli);
        //var deli3 = new List<string>IEnumerable2(ie);
        var result = text.Split(deli, stringSplitOptions).ToList();
        CA.Trim(result);
        if (stringSplitOptions == StringSplitOptions.RemoveEmptyEntries)
        {
            result = result.Where(d => d.Trim() != string.Empty).ToList();
        }

        return result;
    }




    internal static List<string> SplitByWhiteSpaces(string innerText)
    {
        WhitespaceCharService whitespaceChar = new();
        return innerText.Split(whitespaceChar.whiteSpaceChars.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    internal static Tuple<string, string> SplitFromReplaceManyFormat(string input)
    {
        StringBuilder to = new StringBuilder();
        StringBuilder from = new StringBuilder();

        if (input.Contains("->"))
        {
            var lines = SHGetLines.GetLines(input);

            lines = lines.ConvertAll(d => d.Trim());

            foreach (var item in lines)
            {
                var p = SHSplit.Split(item, "->");
                from.AppendLine(p[0]);
                to.AppendLine(p[1]);
            }
        }
        else
        {
            from.AppendLine(input);
        }


        return new Tuple<string, string>(from.ToString(), to.ToString());

    }

    internal static Tuple<List<string>, List<string>> SplitFromReplaceManyFormatList(string input)
    {
        var t = SplitFromReplaceManyFormat(input);
        return new Tuple<List<string>, List<string>>(SHGetLines.GetLines(t.Item1), SHGetLines.GetLines(t.Item2));
    }
}