namespace SunamoDevCode._sunamo;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class DefaultPaths
{
    //private const string bpBb = @"D:\Documents\BitBucket\";
    public static bool IsIgnored(string p, string bpBb)
    {
        if (p.StartsWith(bpBb)) return true;
        return false;
    }
}