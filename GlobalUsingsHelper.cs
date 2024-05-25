using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunamoDevCode;
public class GlobalUsingsHelper
{
    const string globalUsing = "global using ";
    const string global = "global ";

    public static ParseGlobalUsingsResult Parse(List<string> content)
    {
        ParseGlobalUsingsResult result = new ParseGlobalUsingsResult();
        foreach (var item in content)
        {
            if (item.StartsWith(globalUsing))
            {
                result.GlobalUsings.Add(item.Replace(globalUsing, ""));
            }

            if (item.StartsWith(global))
            {
                var kvp = ParseSymbol(item);
                result.GlobalSymbols.Add(kvp.Key, kvp.Value);
            }
        }

        return result;
    }

    private static KeyValuePair<string, string> ParseSymbol(string item)
    {
        var p = item.Split('=');
        return new KeyValuePair<string, string>(p[0].Replace(global, ""), p[1]);
    }
}

