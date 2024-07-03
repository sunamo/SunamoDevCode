using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunamoDevCode;
/// <summary>
/// Druhá část je v ToNugets
/// Vložit sem jak toto šílenství s nugety skončí
/// </summary>
public class GlobalUsingsHelper
{
    public const string globalUsing = "global using ";
    public const string global = "global ";

    public static ParseGlobalUsingsResult Parse(List<string> content)
    {
        ParseGlobalUsingsResult result = new ParseGlobalUsingsResult();
        foreach (var item in content)
        {
            if (item.StartsWith(globalUsing))
            {
                result.GlobalUsings.Add(item.Replace(globalUsing, ""));
            }
            else if (item.StartsWith(global))
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
        try
        {
            return new KeyValuePair<string, string>(p[0].Replace(global, ""), p[1]);
        }
        catch (global::System.Exception ex)
        {

            throw;
        }

        return new KeyValuePair<string, string>();
    }
}

