namespace SunamoDevCode._sunamo.SunamoStringSplit;

/// <summary>
/// String Helper for splitting strings.
/// </summary>
internal class SHSplit
{
    /// <summary>
    /// Splits a string by specified delimiters, removing empty entries.
    /// </summary>
    /// <param name="text">The text to split.</param>
    /// <param name="delimiters">Delimiters to split by.</param>
    /// <returns>List of split parts.</returns>
    internal static List<string> Split(string text, params string[] delimiters)
    {
        return text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    /// <summary>
    /// Splits a string into a specific number of parts.
    /// Returns null if count of parts is under the required amount.
    /// Automatically adds empty padding items at end if got lower than required.
    /// Automatically joins overloaded items to last part.
    /// </summary>
    /// <param name="what">The text to split.</param>
    /// <param name="parts">Required number of parts.</param>
    /// <param name="delimiter">Delimiter to split by.</param>
    /// <returns>List with exact number of parts, or null if insufficient parts.</returns>
    internal static List<string> SplitToParts(string what, int parts, string delimiter)
    {
        var text = Split(what.RemoveInvisibleChars(), delimiter);
        if (text.Count < parts)
        {
            // Pokud je pocet ziskanych partu mensi, vlozim do zbytku prazdne retezce
            if (text.Count > 0)
            {
                var paddedResult = new List<string>();
                for (var i = 0; i < parts; i++)
                    if (i < text.Count)
                        paddedResult.Add(text[i]);
                    else
                        paddedResult.Add("");
                return paddedResult;
                //return new string[] { text[0] };
            }

            return null;
        }

        if (text.Count == parts)
            // Pokud pocet ziskanych partu souhlasim presne, vratim jak je
            return text;
        // Pokud je pocet ziskanych partu vetsi nez kolik ma byt, pripojim ty co josu navic do zbytku
        parts--;
        var result = new List<string>();
        for (var i = 0; i < text.Count; i++)
            if (i < parts)
                result.Add(text[i]);
            else if (i == parts)
                result.Add(text[i] + delimiter);
            else if (i != text.Count - 1)
                result[parts] += text[i] + delimiter;
            else
                result[parts] += text[i];
        return result;
    }

    /// <summary>
    /// Splits a string by a single character delimiter.
    /// </summary>
    /// <param name="text">The text to split.</param>
    /// <param name="delimiter">The character delimiter.</param>
    /// <returns>List of split parts.</returns>
    internal static List<string> SplitChar(string text, char delimiter)
    {
        return Split(StringSplitOptions.RemoveEmptyEntries, text, (new List<char>([delimiter]).ConvertAll(character => character.ToString()).ToArray()));
    }

    /// <summary>
    /// Splits a string by specified delimiters with custom split options.
    /// </summary>
    /// <param name="stringSplitOptions">Options for splitting behavior.</param>
    /// <param name="text">The text to split.</param>
    /// <param name="delimiter">Delimiters to split by.</param>
    /// <returns>List of trimmed split parts.</returns>
    internal static List<string> Split(StringSplitOptions stringSplitOptions, string text, params string[] delimiter)
    {
        if (delimiter == null || delimiter.Length == 0)
        {
            throw new Exception("NoDelimiterDetermined");
        }
        var result = text.Split(delimiter, stringSplitOptions).ToList();
        CA.Trim(result);
        if (stringSplitOptions == StringSplitOptions.RemoveEmptyEntries)
        {
            result = result.Where(line => line.Trim() != string.Empty).ToList();
        }

        return result;
    }




    /// <summary>
    /// Splits a string by all whitespace characters.
    /// </summary>
    /// <param name="text">The text to split.</param>
    /// <returns>List of parts split by whitespace.</returns>
    internal static List<string> SplitByWhiteSpaces(string text)
    {
        WhitespaceCharService whitespaceChar = new();
        return text.Split(whitespaceChar.WhiteSpaceChars.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    /// <summary>
    /// Splits replacement format strings into from and to parts.
    /// Format: "from->to" on each line.
    /// </summary>
    /// <param name="input">Input string in replacement format.</param>
    /// <returns>Tuple with from and to strings.</returns>
    internal static Tuple<string, string> SplitFromReplaceManyFormat(string input)
    {
        StringBuilder to = new StringBuilder();
        StringBuilder from = new StringBuilder();

        if (input.Contains("->"))
        {
            var lines = SHGetLines.GetLines(input);

            lines = lines.ConvertAll(line => line.Trim());

            foreach (var item in lines)
            {
                var parts = SHSplit.Split(item, "->");
                from.AppendLine(parts[0]);
                to.AppendLine(parts[1]);
            }
        }
        else
        {
            from.AppendLine(input);
        }

        return new Tuple<string, string>(from.ToString(), to.ToString());
    }

    /// <summary>
    /// Splits replacement format strings into from and to lists.
    /// </summary>
    /// <param name="input">Input string in replacement format.</param>
    /// <returns>Tuple with from and to lists.</returns>
    internal static Tuple<List<string>, List<string>> SplitFromReplaceManyFormatList(string input)
    {
        var temp = SplitFromReplaceManyFormat(input);
        return new Tuple<List<string>, List<string>>(SHGetLines.GetLines(temp.Item1), SHGetLines.GetLines(temp.Item2));
    }
}