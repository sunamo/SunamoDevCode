namespace SunamoDevCode._sunamo;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class SHSubstring
{
    public static string SubstringIfAvailableStart(string name, int v1)
    {
        if (name.Length > v1) return name.Substring(v1);

        return name;
    }
}