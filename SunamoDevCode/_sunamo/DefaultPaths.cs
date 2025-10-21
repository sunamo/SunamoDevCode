// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoDevCode._sunamo;

internal class DefaultPaths
{
    //private const string bpBb = @"D:\Documents\BitBucket\";
    internal static bool IsIgnored(string p, string bpBb)
    {
        if (p.StartsWith(bpBb)) return true;
        return false;
    }
}