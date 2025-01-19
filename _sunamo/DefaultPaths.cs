namespace SunamoDevCode._sunamo;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class DefaultPaths
{
    public const string eVsProjectsPinp = @"E:\vs\Projects\PlatformIndependentNuGetPackages\";

    public const string bpBb = @"D:\Documents\BitBucket\";
    public static bool IsIgnored(string p)
    {
        if (p.StartsWith(bpBb)) return true;
        return false;
    }

    public static readonly string eVsProjects = @"E:\vs\Projects\";
    public const string eVs = @"E:\vs\";
}