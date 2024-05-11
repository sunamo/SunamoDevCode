


namespace SunamoDevCode;
internal class SHSplit
{
    internal static List<string> Split(string p, params string[] newLine)
    {
        return p.Split(newLine, StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    internal static List<string> SplitByWhiteSpaces(string innerText)
    {
        return innerText.Split(AllChars.whiteSpacesChars.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    internal static Tuple<string, string> SplitFromReplaceManyFormat(string input)
    {
        StringBuilder to = new StringBuilder();
        StringBuilder from = new StringBuilder();

        if (input.Contains(Consts.transformTo))
        {

            var lines = SHSE.GetLines(input);

            lines = lines.ConvertAll(d => d.Trim());

            foreach (var item in lines)
            {
                var p = SHSplit.Split(item, Consts.transformTo);
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
        return new Tuple<List<string>, List<string>>(SHSE.GetLines(t.Item1), SHSE.GetLines(t.Item2));
    }

    internal static List<string> SplitChar(string parametry, char deli)
    {
        return Split(StringSplitOptions.RemoveEmptyEntries, parametry, (new List<char>([deli]).ConvertAll(d => d.ToString()).ToArray()));
    }

    internal static List<string> Split(StringSplitOptions stringSplitOptions, string text, params string[] deli)
    {
        if (deli == null || deli.Count() == 0)
        {
            throw new Exception("NoDelimiterDetermined");
        }
        //var ie = CA.OneElementCollectionToMulti(deli);
        //var deli3 = new List<string>IEnumerable2(ie);
        var result = text.Split(deli, stringSplitOptions).ToList();
        CASE.Trim(result);
        if (stringSplitOptions == StringSplitOptions.RemoveEmptyEntries)
        {
            result = result.Where(d => d.Trim() != string.Empty).ToList();
        }

        return result;
    }
}
